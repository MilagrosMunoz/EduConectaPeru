using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }

        [Required(ErrorMessage = "El nombre del banco es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre del Banco")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Payment>? Payments { get; set; }
    }
}