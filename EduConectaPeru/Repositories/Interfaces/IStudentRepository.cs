using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;

namespace EduConectaPeru.Repositories.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsWithLegalGuardianAsync();
        Task<IEnumerable<Student>> GetStudentsByGradeAsync(int gradoSeccionId);
        Task<Student> GetStudentByDNIAsync(string dni);
        Task<IEnumerable<Student>> GetActiveStudentsAsync();
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
    }
}