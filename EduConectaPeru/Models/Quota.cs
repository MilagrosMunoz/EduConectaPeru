using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Quota
    {
        [Key]
        public int QuotaId { get; set; }

        [Required(ErrorMessage = "El estudiante es obligatorio")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "La matrícula es obligatoria")]
        public int MatriculaId { get; set; }

        [Required(ErrorMessage = "El grado/sección es obligatorio")]
        public int GradoSeccionId { get; set; }

        [Required(ErrorMessage = "El mes es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Mes")]
        public string Mes { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El estado de pago es obligatorio")]
        [Display(Name = "Estado de Pago")]
        public int PaymentStatusId { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("MatriculaId")]
        public virtual Matricula? Matricula { get; set; }

        [ForeignKey("GradoSeccionId")]
        public virtual GradoSeccion? GradoSeccion { get; set; }

        [ForeignKey("PaymentStatusId")]
        public virtual PaymentStatus? PaymentStatus { get; set; }
    }
}