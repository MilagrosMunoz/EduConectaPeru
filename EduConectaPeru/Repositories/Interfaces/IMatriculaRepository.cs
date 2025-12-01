using EduConectaPeru.Models;

namespace EduConectaPeru.Repositories.Interfaces
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task<IEnumerable<Matricula>> GetMatriculasWithDetailsAsync();
        Task<Matricula> GetMatriculaByIdWithDetailsAsync(int id);
        Task<IEnumerable<Matricula>> GetMatriculasByStudentAsync(int studentId);
        Task<IEnumerable<Matricula>> GetMatriculasByYearAsync(int year);
        Task<IEnumerable<Matricula>> GetActiveMatriculasAsync();
        Task<bool> StudentHasActiveMatriculaAsync(int studentId);
    }
}