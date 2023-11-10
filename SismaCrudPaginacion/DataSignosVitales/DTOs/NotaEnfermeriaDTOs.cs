using DataSignosVitales.Entities.NotaEnfermeriaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataSignosVitales.DTOs
{
    public class NotaEnfermeriaDTOs
    {

        [MaxLength(40, ErrorMessage = " longitud maxima permita de 40")]
        public string Numero { get; set; } = null!;

        public int Ingreso { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Hora { get; set; }

        [JsonIgnore]
        public string? Resumen { get; set; } = "";

        public string? Enfermera { get; set; }

        public string? CodigoEnfermera { get; set; }

        public string? TensionArterial { get; set; }

        public string? FrecuenciaCardiaca { get; set; }

        public string? FrecuenciaRespiratoria { get; set; }

        public string? Peso { get; set; }

        public decimal? Temperatura { get; set; }

        public string? Oxigeno { get; set; }

        [JsonIgnore]
        public int? SoloVitales { get; set; } = 1;

        [JsonIgnore]
        public bool Cerrada { get; set; } = false;
        public decimal? Glucometria { get; set; }

        public int? UnidadFuncional { get; set; }

        public string? Tamizaje { get; set; }

    

    }
}
