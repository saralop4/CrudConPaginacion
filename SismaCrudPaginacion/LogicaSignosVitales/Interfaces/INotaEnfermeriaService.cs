using DataSignosVitales.DTOs;
using DataSignosVitales.Entities.NotaEnfermeriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaSignosVitales.Interfaces
{
    public  interface INotaEnfermeriaService 
    {
        Task<(IEnumerable<NotaEnfermeriaDTOs>, int)> GetNotaEnfermeria(int page = 1, int pageSize = 10, string numero = null, DateTime? fecha = null, string codEnfermera = null);
        Task<NotaEnfermeriaDTOs?> GetNotaEnfermera(string numero);
        Task<NotaEnfermeriaDTOs> CreateNota(NotaEnfermeriaDTOs notaEnfermeriaDtos);
        Task UpdateNota(string numero, NotaEnfermeriaDTOs notaEnfermeriaDtos);
        Task DeleteNota(string numero);
        Task<IEnumerable<NotasEnfermerium>> GetAll();

    }
}

