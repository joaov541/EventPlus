using Azure;
using Azure.AI.ContentSafety;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioEventoController : ControllerBase
{
    private readonly ContentSafetyClient _contentSafetyClient;

    private readonly IComentarioEventoRepository _comentarioEventoRepository;

    public ComentarioEventoController(ContentSafetyClient contentSafetyClient, IComentarioEventoRepository comentarioEventoRepository)
    {
        _contentSafetyClient = contentSafetyClient;
        _comentarioEventoRepository = comentarioEventoRepository;
    }

    /// <summary>
    /// Endpoint para cadastra e modera um comentário
    /// </summary>
    /// <param name="comentarioEvento">Comentário a ser moderado</param>
    /// <returns>Status Code 202 e o comentário criado</returns>
    [HttpPost]
    public async Task<IActionResult> Cadastrar(ComentarioEventoDTO comentarioEvento)
    {
        try
        {
            if (string.IsNullOrEmpty(comentarioEvento.Descricao))
            {
                return BadRequest("O texto a ser moderado não pode estar vazio.");
            }

            //Criar objeto de analise
            var request = new AnalyzeTextOptions(comentarioEvento.Descricao);

            //Chamar a API do Azure Content Safety para analisar o texto
            Response<AnalyzeTextResult> response = await _contentSafetyClient.AnalyzeTextAsync(request);

            //Verificar se o texto tem alguma severidade maior que 0
            bool temConteudoImproprio = response.Value.CategoriesAnalysis.Any(comentario => comentario.Severity > 0);

            var novoComentario = new ComentarioEvento
            {
                Descricao = comentarioEvento.Descricao,
                IdUsuario = comentarioEvento.IdUsuario,
                IdEvento = comentarioEvento.IdEvento,
                DataComentarioEvento = DateTime.Now,
                //Define se o comentário vai ser exibido
                Exibe = !temConteudoImproprio
            };

            //Cadastrar o comentário no banco de dados
            _comentarioEventoRepository.Cadastrar(novoComentario);

            return StatusCode(201, novoComentario);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }
    }


    /// <summary>
    /// Endpoint para buscar um comentário por id do usuário e id do evento
    /// </summary>
    /// <param name="idUsuario">Id do usuário a ser buscado</param>
    /// <param name="idEvento">Id do evento a ser buscado</param>
    /// <returns></returns>
    [HttpGet("{idUsuario}/{idEvento}")]
    public IActionResult BuscarPorUsuarioEvento(Guid idUsuario, Guid idEvento)
    {
        try
        {
            var comentario = _comentarioEventoRepository.BuscarPorIdUsuario(idUsuario, idEvento);

            if (comentario == null)
            {
                return NotFound("Comentário não encontrado.");
            }

            return Ok(comentario);
        }
        catch (Exception e)
        {

            return BadRequest(e.Message);
        }

    }

    /// <summary>
    /// Endpoint para listar os comentários de um evento
    /// </summary>
    /// <param name="IdEvento">Id do evento listado</param>
    /// <returns>Status code 200 e lista de comentários do evento</returns>
    [HttpGet("ListarComentariosEvento/{IdEvento}")]
    public IActionResult ListarComentariosEvento(Guid IdEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.Listar(IdEvento));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    /// <summary>
    /// Endpoint para listar somente os comentários que tem a propriedade Exibe como true, ou seja, os comentários que não tem conteúdo impróprio
    /// </summary>
    /// <param name="IdEvento">Id do evento que exibe apenas true</param>
    /// <returns>Status code 200 e lista de comentários true no evento</returns>
    [HttpGet("ExibeTrue/{IdEvento}")]
    public IActionResult ListarSomenteExibe(Guid IdEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.ListarSomenteExibe(IdEvento));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }

    /// <summary>
    /// Endpoint para deletar um comentário por id do comentário
    /// </summary>
    /// <param name="IdComentarioEvento">Id do comentário deletado</param>
    /// <returns>Status code 200 e comentário deletado</returns>
    [HttpDelete("{IdComentarioEvento}")]
    public IActionResult Deletar(Guid IdComentarioEvento)
    {
        try
        {
            _comentarioEventoRepository.Deletar(IdComentarioEvento);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
