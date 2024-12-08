using System;
using System.ComponentModel.DataAnnotations;

namespace albanaPlayaEst.Dto
{
    public class AsignarVDto
    {
        public int Cod_esp { get; set; }
        public bool Estad_esp { get; set; }
        public string Ubi_esp { get; set; }
        public string Descr_esp { get; set; } // Descripción del espacio
        public decimal Cost_esp { get; set; } // Costo del espacio
        public int? CodReg { get; set; }
        
        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        public DateTime? FechaEntrada { get; set; }

        public DateTime? FechaHoraSalida { get; set; }
        public int CodV { get; set; }
        public string MarcV { get; set; }
        public string ModelV { get; set; }
        public string ColorV { get; set; }

        [Required(ErrorMessage = "La placa del vehículo es obligatoria.")]
        public string PlacaV { get; set; }

        public int CodTipV { get; set; }
        public string DescrTipV { get; set; }

        [Required(ErrorMessage = "El código de cliente es obligatorio.")]
        public int Cod_cliente { get; set; } // auto_increment

        public string Nom_cliente { get; set; }
        public string Apell_cliente { get; set; }
        public string Tel_cliente { get; set; }

        [Required(ErrorMessage = "El DNI del cliente es obligatorio.")]
        public string Dni_cliente { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public int CodMetd { get; set; } // Código único del método de pago

        public int? cod_Pag { get; set; } 
        public decimal MontPag { get; set; } // Monto del pago

        // Nuevos campos
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaFin { get; set; } 
        public bool EsPagoRecurrente { get; set; } = false;
        public bool Estado { get; set; } = false;

        public VehiculoDto Vehiculo { get; set; } // Para el vehículo
        public EspacioDto Espacio { get; set; }   // Para el espacio
        public PagoDto Pago { get; set; } // Para el pago
    }
}
