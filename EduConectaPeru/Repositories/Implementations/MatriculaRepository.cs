using Microsoft.EntityFrameworkCore;
using EduConectaPeru.Data;
using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;

namespace EduConectaPeru.Repositories.Implementations
{
    public class MatriculaRepository : Repository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasWithDetailsAsync()
        {
            return await _dbSet
                .Include(m => m.Student)
                .Include(m => m.LegalGuardian)
                .Include(m => m.GradoSeccion)
                .Include(m => m.Docente)
                .Include(m => m.Horario)
                .ToListAsync();
        }

        public async Task<Matricula> GetMatriculaByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Student)
                .Include(m => m.LegalGuardian)
                .Include(m => m.GradoSeccion)
                .Include(m => m.Docente)
                .Include(m => m.Horario)
                .FirstOrDefaultAsync(m => m.MatriculaId == id);
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasByStudentAsync(int studentId)
        {
            return await _dbSet
                .Include(m => m.GradoSeccion)
                .Include(m => m.Docente)
                .Where(m => m.StudentId == studentId)
                .OrderByDescending(m => m.FechaMatricula)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetMatriculasByYearAsync(int year)
        {
            return await _dbSet
                .Include(m => m.Student)
                .Include(m => m.GradoSeccion)
                .Where(m => m.AnioAcademico == year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetActiveMatriculasAsync()
        {
            return await _dbSet
                .Include(m => m.Student)
                .Include(m => m.GradoSeccion)
                .Include(m => m.LegalGuardian)
                .Where(m => m.Estado == "Activa")
                .ToListAsync();
        }

        public async Task<bool> StudentHasActiveMatriculaAsync(int studentId)
        {
            return await _dbSet
                .AnyAsync(m => m.StudentId == studentId && m.Estado == "Activa");
        }
    }
}