using EduConectaPeru.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de pago es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo de Pago")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
