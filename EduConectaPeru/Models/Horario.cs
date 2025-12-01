using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUCONECTA_PERU.Models
{
    public class Horario
    {
        [Key]
        public int HorarioId { get; set; }

        [Required(ErrorMessage = "El docente es obligatorio")]
        [Display(Name = "Docente")]
        public int DocenteId { get; set; }

        [Required(ErrorMessage = "El grado/sección es obligatorio")]
        [Display(Name = "Grado/Sección")]
        public int GradoSeccionId { get; set; }

        [Required(ErrorMessage = "El curso es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Curso")]
        public string Curso { get; set; } = string.Empty;

        [Required(ErrorMessage = "El día de la semana es obligatorio")]
        [StringLength(15)]
        [Display(Name = "Día de la Semana")]
        public string DiaSemana { get; set; } = string.Empty;

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [Display(Name = "Hora de Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [Display(Name = "Hora de Fin")]
        public TimeSpan HoraFin { get; set; }


        [Required(ErrorMessage = "La materia es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Materia")]
        public string Materia { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [ForeignKey("DocenteId")]
        public virtual Docente? Docente { get; set; }

        [ForeignKey("GradoSeccionId")]
        public virtual GradoSeccion? GradoSeccion { get; set; }

        public virtual ICollection<Matricula>? Matriculas { get; set; }
        public virtual ICollection<AsignacionDocente>? AsignacionesDocentes { get; set; }
    }
}