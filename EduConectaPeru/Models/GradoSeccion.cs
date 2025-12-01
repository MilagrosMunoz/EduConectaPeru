using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduConectaPeru.Models
{
    public class GradoSeccion
    {
        [Key]
        public int GradoSeccionId { get; set; }

        [Required(ErrorMessage = "El grado es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Grado")]
        public string Grado { get; set; }

        [Required(ErrorMessage = "La sección es obligatoria")]
        [StringLength(10)]
        [Display(Name = "Sección")]
        public string Seccion { get; set; }

        [Required(ErrorMessage = "El nivel es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Nivel")]
        public string Nivel { get; set; } 

        [Display(Name = "Capacidad")]
        public int Capacidad { get; set; } = 30;

        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Grado y Sección")]
        public string NombreCompleto => $"{Grado} - {Seccion} ({Nivel})";

        public virtual ICollection<Horario> Horarios { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; }
    }
}