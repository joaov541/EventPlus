using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly EventContext _context;


    //Método construtor para injeção de dependência do contexto do banco de dados
    public UsuarioRepository(EventContext context)
    {
        _context = context;
    }



    /// <summary>
    /// Busca o usuário pelo e-mail e valida o hash da senha
    /// </summary>
    /// <param name="Email">Enail do usuário a ser buscado</param>
    /// <param name="Senha">Senha para validar o usuário</param>
    /// <returns>Usuário buscado</returns>
    public Usuario BuscarPorEmailESenha(string Email, string Senha)
    {
        //Primeiro, buscamos o usuário pelo e-mail
        var usuarioBuscado = _context.Usuarios.Include(usuario => usuario.IdTipoUsuarioNavigation).FirstOrDefault(usuario => usuario.Email == Email);

        //Verificamos se o usuário foi encontrado
        if (usuarioBuscado != null)
        {
            //Comparamos o hash da senha fornecida com o hash armazenado no banco de dados
            bool confere = Criptografia.CompararHash(Senha, usuarioBuscado.Senha);

            //Se as senhas conferem, retornamos o usuário encontrado
            if (confere)
            {
                return usuarioBuscado;
            }
        }

        return null!;
    }


    /// <summary>
    /// Busca um usuário pelo id incluindo os dados do seu Tipo de Usuário
    /// </summary>
    /// <param name="id">Id do usuário a ser buscado</param>
    /// <returns>Usuário Buscado</returns>
    public Usuario BuscarPorId(Guid id)
    {
        return _context.Usuarios.Include(usuario => usuario.IdTipoUsuarioNavigation).FirstOrDefault(usuario => usuario.IdUsuario == id)!;
    }


    /// <summary>
    /// Cadastra um novo usuário no banco de dados, criptografando a senha antes de salvar, e o Id gerado pelo Banco
    /// </summary>
    /// <param name="usuario">Usuário a ser cadastrado</param>
    public void Cadastrar(Usuario usuario)
    {
        usuario.Senha = Criptografia.GerarHash(usuario.Senha);

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public List<Usuario> Listar()
    {
        return _context.Usuarios.OrderBy(Usuario => Usuario.Nome).ToList();
    }
}

