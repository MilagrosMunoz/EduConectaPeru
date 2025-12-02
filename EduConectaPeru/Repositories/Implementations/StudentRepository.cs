using EduConectaPeru.Data;
using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;
using EduConectaPeru.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EduConectaPeru.Repositories.Implementations
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetStudentsWithLegalGuardianAsync()
        {
            return await _dbSet
                .Include(s => s.LegalGuardian)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByGradeAsync(int gradoSeccionId)
        {
            return await _context.Matriculas
                .Where(m => m.GradoSeccionId == gradoSeccionId && m.Estado == "Activa")
                .Include(m => m.Student)
                    .ThenInclude(s => s.LegalGuardian)
                .Select(m => m.Student)
                .ToListAsync();
        }

        public async Task<Student> GetStudentByDNIAsync(string dni)
        {
            return await _dbSet
                .Include(s => s.LegalGuardian)
                .FirstOrDefaultAsync(s => s.DNI == dni);
        }

        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            var activeStudentIds = await _context.Matriculas
                .Where(m => m.Estado == "Activa")
                .Select(m => m.StudentId)
                .Distinct()
                .ToListAsync();

            return await _dbSet
                .Where(s => activeStudentIds.Contains(s.StudentId))
                .Include(s => s.LegalGuardian)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            return await _dbSet
                .Include(s => s.LegalGuardian)
                .Where(s => s.FirstName.Contains(searchTerm) ||
                           s.LastName.Contains(searchTerm) ||
                           s.DNI.Contains(searchTerm))
                .ToListAsync();
        }
    }
}