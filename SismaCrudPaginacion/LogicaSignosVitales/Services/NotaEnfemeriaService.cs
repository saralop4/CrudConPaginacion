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
    public class NotaEnfemeriaService : INotaEnfermeriaService
    {
        private readonly INotaEnfermeriaDbContext _notaenfermeriadbcontext;

        public NotaEnfemeriaService(INotaEnfermeriaDbContext notaenfermeriadbcontext)
        {
            _notaenfermeriadbcontext = notaenfermeriadbcontext;

        }

        public async Task<NotaEnfermeriaDTOs> CreateNota(NotaEnfermeriaDTOs notaEnfermeriaDtos)
        {
            // Obtenemos el código de enfermera de SisMae basado en el valor CodEnfer de DTO
            var codigoEnfermera = await _notaenfermeriadbcontext.SisMedis
                .Where(sm => sm.Codigo.ToString() == notaEnfermeriaDtos.CodigoEnfermera)
                .Select(sm => sm.Codigo)
                .FirstOrDefaultAsync();

            // Obtenemos el ingreso de SisMedi basado en el valor Ingreso de DTO
            var ingreso = await _notaenfermeriadbcontext.SisMaes
                  .Where(md => md.ConEstudio == notaEnfermeriaDtos.Ingreso)
                  .Select(md => md.ConEstudio)
                  .FirstOrDefaultAsync();


            if (codigoEnfermera.ToString() != null && ingreso.ToString() != null)
            {
                // Si ambos valores existen en las tablas relacionadas, podemos proceder con la inserción
                var IncrementBy = (int source = 0, int increment = 1) => source + increment;
                Console.WriteLine(IncrementBy(5));

                var create = new NotasEnfermerium
                {

                    Numero = IncrementBy(1).ToString(),
                    Ingreso = (int)ingreso,
                    Fecha = notaEnfermeriaDtos.Fecha,
                    Hora = notaEnfermeriaDtos.Hora,
                    Enfermera = notaEnfermeriaDtos.Enfermera,
                    CodEnfer = codigoEnfermera.ToString(),
                    Ta = notaEnfermeriaDtos.TensionArterial,
                    Fc = notaEnfermeriaDtos.FrecuenciaCardiaca,
                    Fr = notaEnfermeriaDtos.FrecuenciaRespiratoria,
                    Ps = notaEnfermeriaDtos.Peso,
                    Tp = notaEnfermeriaDtos.Temperatura,
                    O2 = notaEnfermeriaDtos.Oxigeno,
                    Glucometria = notaEnfermeriaDtos.Glucometria,
                    Ufuncional = notaEnfermeriaDtos.UnidadFuncional,
                    Tam = notaEnfermeriaDtos.Tamizaje,

                };

                // Agregar la entidad a la base de datos y guardar los cambios
                _notaenfermeriadbcontext.NotasEnfermeria.Add(create);
                await _notaenfermeriadbcontext.SaveChangesAsync();

            }
            return notaEnfermeriaDtos;
        }

            public async Task DeleteNota(string numero)
            {
                var buscar = await SearchByNumeroNotaEnfermera(numero);

                if (buscar != null)
                {
                    _notaenfermeriadbcontext.NotasEnfermeria.Remove(buscar);
                    await _notaenfermeriadbcontext.SaveChangesAsync();
                }
            }

            public async Task<IEnumerable<NotasEnfermerium>> GetAll()
            {
                return await _notaenfermeriadbcontext.NotasEnfermeria.ToListAsync();

            }

            public async Task UpdateNota(string numero,NotaEnfermeriaDTOs notaEnfermeriaDtos)
            {
                var buscar = await SearchByNumeroNotaEnfermera(numero);

                Console.WriteLine("lo que se guarda en buscar = " + buscar);

                if (buscar != null)
                {

                //si actualiza solo que ingreso ni numero deben poder actualizarce
                    buscar.Fecha = notaEnfermeriaDtos.Fecha;
                    buscar.Hora = notaEnfermeriaDtos.Hora;
                    buscar.Enfermera = notaEnfermeriaDtos.Enfermera;
                    buscar.CodEnfer = notaEnfermeriaDtos.CodigoEnfermera;
                    buscar.Ta = notaEnfermeriaDtos.TensionArterial;
                    buscar.Fc= notaEnfermeriaDtos.FrecuenciaCardiaca;
                    buscar.Fr = notaEnfermeriaDtos.FrecuenciaRespiratoria;
                    buscar.Ps = notaEnfermeriaDtos.Peso;
                    buscar.Tp = notaEnfermeriaDtos.Temperatura;
                    buscar.O2 = notaEnfermeriaDtos.Oxigeno;
                    buscar.Glucometria = notaEnfermeriaDtos.Glucometria;
                    buscar.Ufuncional = notaEnfermeriaDtos.UnidadFuncional;
                    buscar.Ta = notaEnfermeriaDtos.Tamizaje;

                    await _notaenfermeriadbcontext.SaveChangesAsync();     

                }
            }



            //con este metodo 'SearchByNumeroNotaEnfermera' consulto un objeto por id en este caso por numero
            private async Task<NotasEnfermerium> SearchByNumeroNotaEnfermera(string numero)
            {
                var buscar = await _notaenfermeriadbcontext.NotasEnfermeria.FirstOrDefaultAsync(x => x.Numero == numero);

                if (buscar == null)
                {
                    return null;
                }

                return buscar;
            }


        //paginacion
            public async Task<(IEnumerable<NotaEnfermeriaDTOs>, int)> GetNotaEnfermeria(int page = 1, int pageSize = 10, string numero = null, DateTime? fecha = null, string? codEnfermera = null)
            {


            if (page < 1)
                {
                    page = 1;
                }

                if (pageSize < 1)
                {
                    pageSize = 10;
                }

                var query = _notaenfermeriadbcontext.NotasEnfermeria.AsQueryable();

                if (!string.IsNullOrEmpty(numero))
                {
                    query = query.Where(x => x.Numero.Contains(numero));
                }

                if (fecha != null)
                {
                    query = query.Where(x => x.Fecha == fecha);
                }

                if (codEnfermera != null)
                {
                    query = query.Where(x => x.CodEnfer == codEnfermera);
                }

                var totalDocumentos = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalDocumentos / pageSize);

                var documentos = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var NotaEnfermeriaDTOs = documentos.Select(x => new NotaEnfermeriaDTOs
                {
                    Numero = x.Numero,
                    Ingreso = x.Ingreso,
                    Fecha = x.Fecha,
                    Hora = x.Hora,
                    Enfermera = x.Enfermera,
                    CodigoEnfermera = x.CodEnfer,
                    TensionArterial = x.Ta,
                    FrecuenciaCardiaca = x.Fc,
                    FrecuenciaRespiratoria = x.Fr,
                    Peso = x.Ps,
                    Temperatura = x.Tp,
                    Oxigeno = x.O2,
                    Glucometria = x.Glucometria,
                    UnidadFuncional = x.Ufuncional,
                    Tamizaje = x.Tam,
                });

                return (NotaEnfermeriaDTOs, totalPages);
            }
            

        //metodo que se utiliza unicamente en el controlador.
            public async Task<NotaEnfermeriaDTOs?> GetNotaEnfermera(string numero)
            {
                var v = await _notaenfermeriadbcontext.NotasEnfermeria.FirstOrDefaultAsync(x => x.Numero == numero);

                if (v == null)
                {
                    return null;
                }

            var numeroDtos = new NotaEnfermeriaDTOs
            {
                    Numero = v.Numero,
                    Fecha = v.Fecha,
                    Hora = v.Hora,
                    Enfermera = v.Enfermera,
                    CodigoEnfermera = v.CodEnfer,
                    TensionArterial = v.Ta,
                    FrecuenciaCardiaca = v.Fc,
                    FrecuenciaRespiratoria = v.Fr,
                    Peso = v.Ps,
                    Temperatura = v.Tp,
                    Oxigeno = v.O2,
                    UnidadFuncional = v.Ufuncional,
                    Tamizaje = v.Tam
                };

                return numeroDtos;
            }






    }
    }
