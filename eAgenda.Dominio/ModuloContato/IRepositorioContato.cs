using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloTarefa;
using System.Collections.Generic;

namespace eAgenda.Dominio.ModuloContato
{
    public interface IRepositorioContato : IRepositorio<Contato>
    {
        List<Contato> SelecionarTodos(System.Guid guid);
    }
}
