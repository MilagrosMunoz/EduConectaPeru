using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

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

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(10)]
        [Display(Name = "Género")]
        public string Gender { get; set; }

        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [StringLength(15)]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Display(Name = "Apoderado")]
        public int? LegalGuardianId { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{FirstName} {LastName}";

        [Display(Name = "Edad")]
        public int Edad => DateTime.Now.Year - BirthDate.Year;

        [ForeignKey("LegalGuardianId")]
        public virtual LegalGuardian LegalGuardian { get; set; }

        public virtual ICollection<Matricula> Matriculas { get; set; }
        public virtual ICollection<Quota> Quotas { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}