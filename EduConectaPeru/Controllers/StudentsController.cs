using EduConectaPeru.Data;
using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduConectaPeru.Controllers
{
    [Authorize(Policy = "AdminOrSecretaria")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository<LegalGuardian> _legalGuardianRepository;
        private readonly AppDbContext _context;

        public StudentsController(
            IStudentRepository studentRepository,
            IRepository<LegalGuardian> legalGuardianRepository,
            AppDbContext context)
        {
            _studentRepository = studentRepository;
            _legalGuardianRepository = legalGuardianRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            IEnumerable<Student> students;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                students = await _studentRepository.SearchStudentsAsync(searchTerm);
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                students = await _studentRepository.GetStudentsWithLegalGuardianAsync();
            }

            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var studentWithDetails = await _studentRepository.GetStudentsWithLegalGuardianAsync();
            var student = studentWithDetails.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLegalGuardiansDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                var existingStudent = await _studentRepository.GetStudentByDNIAsync(student.DNI);
                if (existingStudent != null)
                {
                    ModelState.AddModelError("DNI", "Ya existe un estudiante con este DNI");
                    await LoadLegalGuardiansDropdown(student.LegalGuardianId);
                    return View(student);
                }

                student.CreatedAt = DateTime.Now;
                await _studentRepository.AddAsync(student);

                TempData["SuccessMessage"] = "Estudiante registrado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await LoadLegalGuardiansDropdown(student.LegalGuardianId);
                return View(student);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            await LoadLegalGuardiansDropdown(student.LegalGuardianId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            try
            {
                var originalStudent = await _context.Students.FindAsync(id);

                if (originalStudent == null)
                {
                    TempData["ErrorMessage"] = "Estudiante no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                var existingStudent = await _studentRepository.GetStudentByDNIAsync(student.DNI);
                if (existingStudent != null && existingStudent.StudentId != id)
                {
                    ModelState.AddModelError("DNI", "Ya existe otro estudiante con este DNI");
                    await LoadLegalGuardiansDropdown(student.LegalGuardianId);
                    return View(student);
                }

                originalStudent.FirstName = student.FirstName;
                originalStudent.LastName = student.LastName;
                originalStudent.DNI = student.DNI;
                originalStudent.BirthDate = student.BirthDate;
                originalStudent.Gender = student.Gender;
                originalStudent.Phone = student.Phone;
                originalStudent.Address = student.Address;
                originalStudent.LegalGuardianId = student.LegalGuardianId;

                _context.Entry(originalStudent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Estudiante actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                await LoadLegalGuardiansDropdown(student.LegalGuardianId);
                return View(student);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var studentWithDetails = await _studentRepository.GetStudentsWithLegalGuardianAsync();
            var student = studentWithDetails.FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _studentRepository.DeleteAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Estudiante eliminado exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLegalGuardiansDropdown(int? selectedId = null)
        {
            var legalGuardians = await _legalGuardianRepository.GetAllAsync();

            ViewBag.LegalGuardians = new SelectList(
                legalGuardians,
                "LegalGuardianId",
                "NombreCompleto",
                selectedId
            );
        }
    }
}
