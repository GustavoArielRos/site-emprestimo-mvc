
//As pastas que iremos interagir aqui
using EmprestimoLivros.Data;
using EmprestimoLivros.Dto;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {

        //para ter acesso as tabelas do banco de dados
        private readonly ApplicationDbContext _context;

        //usar os métodos dessa interface
        private readonly ISenhaInterface _senhaInterface;

        private readonly ISessaoInterface _sessaoInterface;

        //construtor
        public LoginService(ApplicationDbContext context, ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }

        public Task<ResponseModal<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModal<UsuarioModel> response = new ResponseModal<UsuarioModel>();

            try
            {

                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioLoginDto.Email);

                if(usuario == null)
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }

                //só vai entrar no if quando as senhas NÃO FOREM IGUAIS
                if(!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }

                //Criar uma sessão
                _sessaoInterface.CriarSessao(usuario);

                response.Mensagem = "Usuário logado com sucesso";

                return response;

            }
            catch (Exception ex)
            {

                response.Mensagem = ex.Message;
                response.Status = false;
                return response;

            }
        }

        public async Task<ResponseModal<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto)
        {

            ResponseModal<UsuarioModel> response = new ResponseModal<UsuarioModel>();

            try
            {

                if(VerificarSeEmailExiste(usuarioRegisterDto))
                {
                    response.Mensagem = "Email Já Cadastro";
                    response.Status = false;
                    return response;
                }

                //cria a senha hash voltando a hash e salt já preenchida
                _senhaInterface.CriarSenhaHash(usuarioRegisterDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel(){
                    Nome = usuarioRegisterDto.Nome,
                    Sobrenome = usuarioRegisterDto.Sobrenome,
                    Email = usuarioRegisterDto.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                response.Mensagem = "Usuário Cadastrado Com Sucesso";

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;

                return response;
            }
        }


        private bool VerificarSeEmailExiste(UsuarioRegisterDto u)
        {   
            //vendo se tem um email na tabela que é igual ao que recebe no parâmetro
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == u.Email);

            if(usuario == null)
            {
                return false;
            }

            return true;
        }
    }
}
