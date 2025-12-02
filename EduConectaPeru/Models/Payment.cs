using EduConectaPeru.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "El estudiante es obligatorio")]
        [Display(Name = "Estudiante")]
        public int StudentId { get; set; }

        [Display(Name = "Apoderado")]
        public int? LegalGuardianId { get; set; }

        [Required(ErrorMessage = "El tipo de pago es obligatorio")]
        [Display(Name = "Tipo de Pago")]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "El banco es obligatorio")]
        [Display(Name = "Banco")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "El número de comprobante es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Número de Comprobante")]
        public string NumeroComprobante { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de pago es obligatoria")]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaPago { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Display(Name = "URL Comprobante")]
        [StringLength(500)]
        public string? ComprobanteURL { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("LegalGuardianId")]
        public virtual LegalGuardian? LegalGuardian { get; set; }

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType? PaymentType { get; set; }

        [ForeignKey("BankId")]
        public virtual Bank? Bank { get; set; }
    }
}