using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PresencaController : ControllerBase
{
    private readonly IPresencaRepository _presencaRepository;
    public PresencaController(IPresencaRepository presencaRepository)
    {
        _presencaRepository = presencaRepository;
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para um método de busca de uma presença específica por id
    /// </summary>
    /// <param name="id">id da presença a ser buscada</param>
    /// <returns>Status code 200 e presenca buscada</returns>
    [HttpGet("{id}")]
    public IActionResult BuscarPorId(Guid id)
    {
        try
        {
            return Ok(_presencaRepository.BuscarPorId(id));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que retorna uma lista de presenças filtrada por usuário
    /// </summary>
    /// <param name="IdUsuario">id do usuário para filtragem</param>
    /// <returns>uma lista de presenças filtrada pelo usuário</returns>
    [HttpGet("ListarMinhas/{IdUsuario}")]
    public IActionResult BuscarPoUsuario(Guid IdUsuario)
    {
        try
        {
            return Ok(_presencaRepository.ListarMinhas(IdUsuario));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para um método de criação de uma nova presença, ou seja, inscrição em um evento
    /// </summary>
    /// <param name="presenca">Nome da presença Inscrita</param>
    /// <returns>Status code 201 e a presença cadastrada</returns>
    [HttpPost("Inscrever")]
    public IActionResult Inscrever(PresencaDTO presenca)
    {
        try
        {
            var novaPresenca = new Presenca
            {
                Situacao = presenca.Situacao,
                IdUsuario = presenca.IdUsuario,
                IdEvento = presenca.IdEvento,
            };
            _presencaRepository.Inscrever(novaPresenca);
            return StatusCode(201, novaPresenca);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para um método de atualização de uma presença
    /// </summary>
    /// <param name="id">id da presença atualizada</param>
    /// <param name="presenca">Nome da presença atualizada</param>
    /// <returns>Status code 204 e presença atualizada</returns>
    [HttpPut("{id}")]
    public IActionResult Atualizar(Guid id, PresencaDTO presenca)
    {
        try
        {
            var presencaAtualizada = new Presenca
            {
                Situacao = presenca.Situacao,
                IdUsuario = presenca.IdUsuario,
                IdEvento = presenca.IdEvento,
            };

            _presencaRepository.Atualizar(id, presencaAtualizada);
            return NoContent();
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para um método de deletar uma presença existente no banco de dados
    /// </summary>
    /// <param name="id">id da presença deletada</param>
    /// <returns>Status code</returns>
    [HttpDelete("{id}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            _presencaRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para um método de listar todas as presenças registradas no banco de dados, ordenando-as pela situação (presente ou ausente)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Listar()
    {
        try
        {
            return Ok(_presencaRepository.Listar());
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

}
