using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class LegalGuardian
    {
        [Key]
        public int LegalGuardianId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8)]
        [Display(Name = "DNI")]
        public string DNI { get; set; }

        [StringLength(15)]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{FirstName} {LastName}";

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; }
    }
}