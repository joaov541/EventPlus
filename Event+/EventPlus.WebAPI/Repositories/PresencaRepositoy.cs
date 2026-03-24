using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class PresencaRepositoy : IPresencaRepository
{
    private readonly EventContext _context;

    public PresencaRepositoy(EventContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Método para atualizar uma presença existente no banco de dados.
    /// Ele busca a presença pelo id fornecido
    /// </summary>
    /// <param name="id">id da presença atualizada</param>
    /// <param name="presenca">Nome da presença</param>
    public void Atualizar(Guid id, Presenca presenca)
    {
        var presencaBuscada = _context.Presencas.Find(id);

        if (presencaBuscada != null)
        {
            presencaBuscada.Situacao = !presencaBuscada.Situacao;
            _context.SaveChanges();
        }
    }

    public void Atualizar(Guid id)
    {
        throw new NotImplementedException();
    }




    /// <summary>
    /// Busca uma presença por id
    /// </summary>
    /// <param name="id">id da presença a ser buscada</param>
    /// <returns>Presença buscada</returns>
    public Presenca BuscarPorId(Guid id)
    {
        return _context.Presencas.Include(p => p.IdEventoNavigation).ThenInclude(e => e.IdinstituicaoNavigation).FirstOrDefault(p => p.IdPresenca == id)!;
    }


    /// <summary>
    /// Método para deletar uma presença existente no banco de dados.
    /// </summary>
    /// <param name="id">id da presença a ser deletada</param>
    public void Deletar(Guid id)
    {
        var presencaBuscada = _context.Presencas.Find(id);

        if (presencaBuscada != null)
        {
            _context.Presencas.Remove(presencaBuscada);
            _context.SaveChanges();
        }
    }


    /// <summary>
    /// Método para inscrever um usuário em um evento, criando uma nova presença no banco de dados.
    /// </summary>
    /// <param name="Inscricao">Nome da inscrição inscrita</param>
    public void Inscrever(Presenca Inscricao)
    {
        var novaInscricao = new Presenca
        {
            IdPresenca = Guid.NewGuid(),
            Situacao = Inscricao.Situacao,
            IdUsuario = Inscricao.IdUsuario,
            IdEvento = Inscricao.IdEvento
        };
        _context.Add(novaInscricao);
        _context.SaveChanges();
    }


    /// <summary>
    /// Método para listar todas as presenças registradas no banco de dados, ordenando-as pela situação (presente ou ausente).
    /// </summary>
    /// <returns></returns>
    public List<Presenca> Listar()
    {
        return _context.Presencas.OrderBy(presenca => presenca.Situacao).ToList();
    }


    /// <summary>
    /// Lista as presenças de um usuário específico, incluindo os detalhes do evento e da instituição relacionada a cada presença.
    /// </summary>
    /// <param name="IdUsuario">id do usuário para filtragem</param>
    /// <returns>uma lista de presenças de um usuario específico</returns>
    public List<Presenca> ListarMinhas(Guid IdUsuario)
    {
        return _context.Presencas
             .Include(p => p.IdEventoNavigation)
             .ThenInclude(e => e!.IdinstituicaoNavigation)
             .Where(p => p.IdUsuario == IdUsuario)
             .ToList();
    }
}
