using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly EventContext _context;

    public EventoRepository(EventContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Atualiza o evento por rastreamento automático
    /// </summary>
    /// <param name="id">Id do evento a ser atualizado</param>
    /// <param name="evento">Novos dados do evento</param>
    public void Atualizar(Guid id, Evento evento)
    {
        var EventoBuscado = _context.Eventos.Find(id);

        if (EventoBuscado != null)
        {
            EventoBuscado.Nome = evento.Nome;
            EventoBuscado.Descricao = evento.Descricao;
            EventoBuscado.DataEvento = evento.DataEvento;
            EventoBuscado.IdTipoEvento = evento.IdTipoEvento;
            EventoBuscado.Idinstituicao = evento.Idinstituicao;
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Busca o evento por id
    /// </summary>
    /// <param name="id">Id do evento a ser buscado</param>
    /// <returns></returns>
    public Evento BuscarPorId(Guid id)
    {
        return _context.Eventos.Find(id)!;
    }

    /// <summary>
    /// método que cadastra o evento
    /// </summary>
    /// <param name="evento">nome do evento cadastrado</param>
    public void Cadastrar(Evento evento)
    {
        _context.Eventos.Add(evento);
        _context.SaveChanges();
    }

    /// <summary>
    /// Método para deletar o evento por id
    /// </summary>
    /// <param name="IdEvento">id do evento deletado</param>
    public void Deletar(Guid IdEvento)
    {
        var EventoBuscado = _context.Eventos.Find(IdEvento);

        if (EventoBuscado != null)
        {
            _context.Eventos.Remove(EventoBuscado);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Método de listar os eventos
    /// </summary>
    /// <returns></returns>
    public List<Evento> Listar()
    {
        return _context.Eventos.OrderBy(evento => evento.Nome).ToList();
    }


    /// <summary>
    /// Método que busca eventos no qual um usuário confirmou presença
    /// </summary>
    /// <param name="IdUsuario">Id do usuário a ser buscado</param>
    /// <returns>Uma lista de eventos</returns>
    public List<Evento> ListarPorId(Guid IdUsuario)
    {
        return _context.Eventos.Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdinstituicaoNavigation)
            .Where(e => e.Presencas.Any(p => p.IdUsuario == IdUsuario && p.Situacao == true))
            .ToList();
    }


    /// <summary>
    /// Método que traz a lista de próximos eventos
    /// </summary>
    /// <returns>Uma lista de eventos</returns>
    public List<Evento> ProximosEventos()
    {
        return _context.Eventos.Include(e => e.IdTipoEventoNavigation)
            .Include(e => e.IdinstituicaoNavigation)
            .Where(e => e.DataEvento >= DateTime.Now)
            .OrderBy(e => e.DataEvento)
            .ToList();
    }
}
