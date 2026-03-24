using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Enpoint da API que faz a chamada para um método de busca de um usuário específico por id
    /// </summary>
    /// <param name="id">id do usuário a ser buscado</param>
    /// <returns>Status code 200 e o usuário a ser buscado</returns>
    [HttpGet("{id}")]
    public IActionResult BuscarPorId(Guid id)
    {
        try
        {
            return Ok(_usuarioRepository.BuscarPorId(id));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet]
    public IActionResult Listar()
    {
        try
        {
            return Ok(_usuarioRepository.Listar());
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para um método de cadastro de um novo usuário
    /// </summary>
    /// <param name="usuario">Usuário a ser cadastrado</param>
    /// <returns>Status code 201 e usuário cadastrado</returns>
    [HttpPost]
    public IActionResult Cadastrar(UsuarioDTO dto)
    {
        var usuario = new Usuario
        {

            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            IdTipoUsuario = dto.IdTipoUsuario
        };

        try
        {
            _usuarioRepository.Cadastrar(usuario);
            return StatusCode(201, usuario);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }
}
