using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EduConectaPeru.Data;
using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;

namespace EduConectaPeru.Controllers
{
    [Authorize(Policy = "AdminOrSecretaria")]
    public class MatriculasController : Controller
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly AppDbContext _context;

        public MatriculasController(
            IMatriculaRepository matriculaRepository,
            AppDbContext context)
        {
            _matriculaRepository = matriculaRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, int? year)
        {
            var matriculas = await _matriculaRepository.GetMatriculasWithDetailsAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                matriculas = matriculas.Where(m =>
                    m.Student.FirstName.Contains(searchTerm) ||
                    m.Student.LastName.Contains(searchTerm) ||
                    m.Student.DNI.Contains(searchTerm));
            }

            if (year.HasValue)
            {
                matriculas = matriculas.Where(m => m.AnioAcademico == year.Value);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Year = year;
            ViewBag.AvailableYears = matriculas.Select(m => m.AnioAcademico).Distinct().OrderByDescending(y => y);

            return View(matriculas.OrderByDescending(m => m.FechaMatricula));
        }

        public async Task<IActionResult> Details(int id)
        {
            var matricula = await _matriculaRepository.GetMatriculaByIdWithDetailsAsync(id);
            if (matricula == null) return NotFound();
            return View(matricula);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Matricula matricula)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.LegalGuardian)
                    .FirstOrDefaultAsync(s => s.StudentId == matricula.StudentId);

                if (student == null)
                {
                    ModelState.AddModelError("", "Estudiante no encontrado");
                    await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                    return View(matricula);
                }

                if (!student.LegalGuardianId.HasValue)
                {
                    ModelState.AddModelError("", "El estudiante no tiene un apoderado asignado. Por favor, asigne un apoderado primero.");
                    await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                    return View(matricula);
                }

                matricula.LegalGuardianId = student.LegalGuardianId.Value;

                var hasActiveMatricula = await _matriculaRepository.StudentHasActiveMatriculaAsync(matricula.StudentId);
                if (hasActiveMatricula)
                {
                    ModelState.AddModelError("", "El estudiante ya tiene una matrícula activa");
                    await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                    return View(matricula);
                }

                matricula.FechaMatricula = DateTime.Now;
                matricula.Estado = "Activa";

                await _matriculaRepository.AddAsync(matricula);

                await GenerarCuotasAutomaticas(matricula);

                TempData["SuccessMessage"] = "Matrícula registrada exitosamente y cuotas generadas";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                return View(matricula);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var matricula = await _matriculaRepository.GetByIdAsync(id);
            if (matricula == null) return NotFound();

            await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
            return View(matricula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Matricula matricula)
        {
            if (id != matricula.MatriculaId) return NotFound();

            try
            {
                var original = await _context.Matriculas.FindAsync(id);
                if (original == null) return NotFound();

                var student = await _context.Students
                    .Include(s => s.LegalGuardian)
                    .FirstOrDefaultAsync(s => s.StudentId == matricula.StudentId);

                if (student == null || !student.LegalGuardianId.HasValue)
                {
                    ModelState.AddModelError("", "El estudiante no tiene un apoderado asignado");
                    await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                    return View(matricula);
                }

                original.StudentId = matricula.StudentId;
                original.LegalGuardianId = student.LegalGuardianId.Value;
                original.GradoSeccionId = matricula.GradoSeccionId;
                original.AnioAcademico = matricula.AnioAcademico;
                original.MontoMatricula = matricula.MontoMatricula;
                original.Estado = matricula.Estado;

                _context.Entry(original).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Matrícula actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await LoadDropdowns(matricula.StudentId, matricula.GradoSeccionId);
                return View(matricula);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var matricula = await _matriculaRepository.GetMatriculaByIdWithDetailsAsync(id);
            if (matricula == null) return NotFound();
            return View(matricula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var quotas = await _quotaRepository.FindAsync(q => q.MatriculaId == id);
                foreach (var quota in quotas)
                {
                    await _quotaRepository.DeleteAsync(quota.QuotaId);
                }

                await _matriculaRepository.DeleteAsync(id);

                TempData["SuccessMessage"] = "Matrícula y cuotas asociadas eliminadas exitosamente";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns(int? selectedStudentId = null, int? selectedGradoSeccionId = null)
        {
            var students = await _context.Students.Include(s => s.LegalGuardian).ToListAsync();
            var gradoSecciones = await _gradoSeccionRepository.FindAsync(g => g.IsActive);

            ViewBag.Students = new SelectList(students, "StudentId", "NombreCompleto", selectedStudentId);
            ViewBag.GradoSecciones = new SelectList(gradoSecciones, "GradoSeccionId", "NombreCompleto", selectedGradoSeccionId);
        }

        private async Task GenerarCuotasAutomaticas(Matricula matricula)
        {
            decimal montoCuota = 200; 

            string[] meses = { "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            for (int i = 0; i < 10; i++)
            {
                var quota = new Quota
                {
                    StudentId = matricula.StudentId,
                    MatriculaId = matricula.MatriculaId,
                    GradoSeccionId = matricula.GradoSeccionId,
                    Mes = meses[i],
                    Anio = matricula.AnioAcademico,
                    Monto = montoCuota,
                    FechaVencimiento = new DateTime(matricula.AnioAcademico, i + 3, 10),
                    PaymentStatusId = 1,
                    CreatedAt = DateTime.Now
                };

                await _quotaRepository.AddAsync(quota);
            }
        }
    }
}