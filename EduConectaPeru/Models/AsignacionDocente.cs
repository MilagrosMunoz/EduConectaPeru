using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class AsignacionDocente
    {
        [Key]
        public int AsignacionId { get; set; }

        [Required(ErrorMessage = "El docente es obligatorio")]
        [Display(Name = "Docente")]
        public int DocenteId { get; set; }

        [Required(ErrorMessage = "El horario es obligatorio")]
        [Display(Name = "Horario")]
        public int HorarioId { get; set; }

        [Display(Name = "Fecha de Asignación")]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("DocenteId")]
        public virtual Docente Docente { get; set; }

        [ForeignKey("HorarioId")]
        public virtual Horario Horario { get; set; }
    }
}