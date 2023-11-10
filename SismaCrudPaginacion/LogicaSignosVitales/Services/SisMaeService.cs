using DataSignosVitales.Data;
using DataSignosVitales.DTOs;
using DataSignosVitales.Entities.NotaEnfermeriaModels;
using DataSignosVitales.Interfaces;
using LogicaSignosVitales.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSignosVitales.Services
{
    public class SisMaeService : ISisMaeService
    {
        private readonly INotaEnfermeriaDbContext _context;

        public SisMaeService(INotaEnfermeriaDbContext context)
        {
            _context = context; 
        }

        public async Task<SisMaeDTOs> CreateSisMae(SisMaeDTOs sisMaeDtos)
        {
            var sisMae = _context.SisMaes.Add(new SisMae()
            {
                ConEstudio = sisMaeDtos.ConEstudio,
                TipoEstudio = sisMaeDtos.TipoEstudio,
                CodEspeci = sisMaeDtos.CodEspecialista,
                ViaIngreso = sisMaeDtos.ViaIngreso,
                FechaIng = sisMaeDtos.FechaIngreso,
                FechaEgr = sisMaeDtos.FechaEgreso,
                HoraIng = sisMaeDtos.HoraIngreso,
                HoraEgr = sisMaeDtos.HoraEgreso,
                NroAutoriza = sisMaeDtos.NumeroAutorizacion,
                DiagnoIng = sisMaeDtos.DiagnosticoIngreso

            });


            await _context.SaveChangesAsync();

            return sisMaeDtos; // La admisión se realizó con éxito
        }

        public async Task<SisMaeDTOs?> GetSisMae(int codigoEstudio)
        {
            var objetoEstudio = await _context.SisMaes.FirstOrDefaultAsync(x => x.ConEstudio == codigoEstudio);

            if (objetoEstudio == null)
            {

                return null;
            }

                var ingresoDtos = new SisMaeDTOs
                {
                    ConEstudio = objetoEstudio.ConEstudio
        
                };

            return ingresoDtos;
        }

    }

}

