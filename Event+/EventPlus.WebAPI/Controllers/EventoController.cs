using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventoController : ControllerBase
{
    private readonly IEventoRepository _eventoRepository;

    public EventoController(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para um método de busca de um evento específico por id
    /// </summary>
    /// <param name="IdUsuario">Id do usuário para filtragem</param>
    /// <returns>Status code 200 e uma lista de eventos</returns>
    [HttpGet("Usuario/{IdUsuario}")]
    public IActionResult ListarPorId(Guid IdUsuario)
    {
        try
        {
            return Ok(_eventoRepository.ListarPorId(IdUsuario));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamadampara o método de listar o próximos eventos
    /// </summary>
    /// <returns>Status code 200 e a lista de eventos</returns>
    [HttpGet("ListarProximos")]
    public IActionResult BuscarProximosEventos()
    {
        try
        {
            return Ok(_eventoRepository.ProximosEventos());
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para o método de cadastrar um novo evento
    /// </summary>
    /// <param name="evento">Nome do evento Cadastrado</param>
    /// <returns>Status code 201 e evento cadastrado</returns>
    [HttpPost("CadastrarProximos")]
    public IActionResult CadastrarProximos(EventoDTO evento)
    {
        try
        {
            var novoEvento = new Evento
            {
                Nome = evento.Nome,
                DataEvento = evento.DataEvento,
                Descricao = evento.Descricao,
                IdTipoEvento = evento.IdTipoEvento,
                Idinstituicao = evento.Idinstituicao
            };

            _eventoRepository.Cadastrar(novoEvento);
            return StatusCode(201, novoEvento);

        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para o método de buscar um evento específico por id
    /// </summary>
    /// <param name="id">Nome do evento buscado</param>
    /// <returns>Status code 204 e evento buscado</returns>
    [HttpGet("BuscarProximosEventosPorId/{id}")]
    public IActionResult BuscarPorId(Guid id)
    {
        try
        {
            var evento = _eventoRepository.BuscarPorId(id);
            if (evento == null)
            {
                return StatusCode(404);
            }
            return Ok(evento);
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para o método de atualizar um evento específico por id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="eventoDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public IActionResult AtualizarEventos(Guid id, EventoDTO eventoDTO)
    {
        try
        {
            var eventoAtualizado = new Evento
            {
                Nome = eventoDTO.Nome!,
                Descricao = eventoDTO.Descricao!,
                DataEvento = eventoDTO.DataEvento,
                Idinstituicao = eventoDTO.Idinstituicao
            };
            _eventoRepository.Atualizar(id, eventoAtualizado);
            return NoContent();
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para o método de deletar um evento específico por id
    /// </summary>
    /// <param name="id">Nome do eento deletado</param>
    /// <returns>Status code 204 e evento deletado</returns>
    [HttpDelete("{id}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            _eventoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

}
