using DataSignosVitales.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSignosVitales.Interfaces
{
    public interface ISisMaeService
    {
        Task<SisMaeDTOs?> GetSisMae(int codigoEstudio);
        Task<SisMaeDTOs> CreateSisMae(SisMaeDTOs sisMaeDtos);
    }
}
