using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Docente
    {
        [Key]
        public int DocenteId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        [Display(Name = "DNI")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; } = string.Empty;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{FirstName} {LastName}";
        public virtual ICollection<Matricula>? Matriculas { get; set; }
        public virtual ICollection<AsignacionDocente>? AsignacionesDocentes { get; set; }
    }
}