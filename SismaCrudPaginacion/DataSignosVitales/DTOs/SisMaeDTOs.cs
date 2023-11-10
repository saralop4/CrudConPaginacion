using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSignosVitales.DTOs
{
    public class SisMaeDTOs
    {
        public long ConEstudio { get; set; }

        public string? TipoEstudio { get; set; }

        public string? CodEspecialista { get; set; }

        public int? ViaIngreso { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaEgreso { get; set; }

        public string? HoraIngreso { get; set; }

        public string? HoraEgreso { get; set; }

        public string? NumeroAutorizacion { get; set; }

        public string? DiagnosticoIngreso { get; set; }


    }
}
