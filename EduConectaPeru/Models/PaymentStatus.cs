using EduConectaPeru.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class PaymentStatus
    {
        [Key]
        public int PaymentStatusId { get; set; }

        [Required(ErrorMessage = "El nombre del estado es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public virtual ICollection<Quota>? Quotas { get; set; }
    }
}