using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class ComentarioEventoRepository : IComentarioEventoRepository
{
    private readonly EventContext _context;

    public ComentarioEventoRepository(EventContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Endpoint para buscar um comentário de um evento por meio do Id do usuário e do Id do evento.
    /// </summary>
    /// <param name="IdUsuario">id do usuário buscado</param>
    /// <param name="IdEvento">id do evento buscado</param>
    /// <returns>Status code 204 e lista de usuarios e eventos</returns>
    public ComentarioEvento BuscarPorIdUsuario(Guid IdUsuario, Guid IdEvento)
    {
        return _context.ComentarioEventos
            .Include(c => c.IdUsuarioNavigation)
            .Include(c => c.IdEventoNavigation)
            .FirstOrDefault(c => c.IdUsuario == IdUsuario && c.IdEvento == IdEvento)!;
    }


    /// <summary>
    /// Endpoint para cadastrar um comentário em um evento.
    /// </summary>
    /// <param name="comentarioEvento">Nome do comentário cadastrado</param>
    public void Cadastrar(ComentarioEvento comentarioEvento)
    {
        _context.ComentarioEventos.Add(comentarioEvento);
        _context.SaveChanges();
    }


    /// <summary>
    /// Endpoint para deletar um comentário de um evento por meio do Id do comentário.
    /// </summary>
    /// <param name="id"></param>
    public void Deletar(Guid id)
    {
        var ComentarioEventoBuscado = _context.ComentarioEventos.Find(id);

        if (ComentarioEventoBuscado != null)
        {
            _context.ComentarioEventos.Remove(ComentarioEventoBuscado);
            _context.SaveChanges();
        }
    }


    /// <summary>
    /// Endpoint para listar os comentários de um evento por meio do Id do evento.
    /// </summary>
    /// <param name="IdEvento">Id do evento para listar os comentários</param>
    /// <returns>status code 200 e lista de comentários</returns>
    public List<ComentarioEvento> Listar(Guid IdEvento)
    {
        return _context.ComentarioEventos.OrderBy(ComentarioEventoBuscado => ComentarioEventoBuscado.Descricao).ToList();
    }

    public object ListarComentariosEvento(Guid idEvento)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Endpoint para listar somente os comentários de um evento que estão com a propriedade "Exibe" marcada como true, por meio do Id do evento.
    /// </summary>
    /// <param name="IdEvento">Id do evento para exibir comentários com propriedade exibe</param>
    /// <returns>Status code 200 e comentários com propriedade exibe</returns>
    public List<ComentarioEvento> ListarSomenteExibe(Guid IdEvento)
    {
        return _context.ComentarioEventos
            .Where(ComentarioEventoBuscado => ComentarioEventoBuscado.Exibe == true)
            .OrderBy(ComentarioEventoBuscado => ComentarioEventoBuscado.Descricao)
            .ToList();
    }
}
