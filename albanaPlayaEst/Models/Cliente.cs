using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace albanaPlayaEst.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_cliente { get; set; } // auto_increment

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nom_cliente { get; set; }

        [Required(ErrorMessage = "El apellido del cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        public string Apell_cliente { get; set; }

        [Required(ErrorMessage = "El teléfono del cliente es obligatorio.")]
        [RegularExpression(@"^\+?[0-9]{9,15}$", ErrorMessage = "El teléfono debe tener entre 9 y 15 dígitos.")]
        public string Tel_cliente { get; set; }

        [Required(ErrorMessage = "El DNI del cliente es obligatorio.")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "El DNI debe tener entre 8 y 12 caracteres.")]
        [RegularExpression(@"^\d{8,12}$", ErrorMessage = "El DNI debe consistir en entre 8 y 12 dígitos numéricos.")]
        public string Dni_cliente { get; set; }
        public virtual ICollection<Vehículo> Vehículo { get; set; }
        public virtual ICollection<Registro> Registros { get; set; }
    }
}
