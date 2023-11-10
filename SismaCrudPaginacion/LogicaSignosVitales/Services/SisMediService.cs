using DataSignosVitales.Data;
using DataSignosVitales.DTOs;
using DataSignosVitales.Entities.NotaEnfermeriaModels;

using DataSignosVitales.Interfaces;
using LogicaSignosVitales.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSignosVitales.Services
{
    public class SisMediService: ISisMediService
    {

        //private readonly INotaEnfermeriaDbContext _notaenfermeriadbcontext;

        //public SisMediService(INotaEnfermeriaDbContext notaenfermeriadbcontext)
        //{
        //    _notaenfermeriadbcontext = notaenfermeriadbcontext;
        //}

        //public async Task<SisMedi> GetSisMedi(SisMediDTOs usuario)
        //{
        //    return await _notaenfermeriadbcontext.SisMedis.SingleOrDefaultAsync(x => x.Codigo == usuario.Codigo);
        //}
    }
}
