using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Matricula
    {
        [Key]
        public int MatriculaId { get; set; }

        [Required(ErrorMessage = "El estudiante es obligatorio")]
        [Display(Name = "Estudiante")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "El apoderado es obligatorio")]
        [Display(Name = "Apoderado")]
        public int LegalGuardianId { get; set; }

        [Required(ErrorMessage = "El grado y sección es obligatorio")]
        [Display(Name = "Grado y Sección")]
        public int GradoSeccionId { get; set; }

        [Display(Name = "Docente")]
        public int? DocenteId { get; set; }

        [Display(Name = "Horario")]
        public int? HorarioId { get; set; }

        [Display(Name = "Fecha de Matrícula")]
        public DateTime FechaMatricula { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El año académico es obligatorio")]
        [Display(Name = "Año Académico")]
        public int AnioAcademico { get; set; }

        [Required(ErrorMessage = "El monto de matrícula es obligatorio")]
        [Display(Name = "Monto de Matrícula")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoMatricula { get; set; }

        [StringLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activa";

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("LegalGuardianId")]
        public virtual LegalGuardian LegalGuardian { get; set; }

        [ForeignKey("GradoSeccionId")]
        public virtual GradoSeccion GradoSeccion { get; set; }

        [ForeignKey("DocenteId")]
        public virtual Docente Docente { get; set; }

        [ForeignKey("HorarioId")]
        public virtual Horario Horario { get; set; }

        public virtual ICollection<Quota> Quotas { get; set; }
    }
}