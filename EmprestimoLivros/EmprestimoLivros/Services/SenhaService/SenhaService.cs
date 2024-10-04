using System.Security.Cryptography;

namespace EmprestimoLivros.Services.SenhaService
{
    public class SenhaService : ISenhaInterface
    {
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {   
            //using serve para depois que realizar o bloco de código dentro dele, iremos descartar as mudança
            //a instancia HMACSHA512 é um algoritmo de hash seguro
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;//armazena a chave(salt) gerado pelo HMACSH512
                //System.Text.Encoding.UTF8.GetBytes(senha), converta a string da senha em um array de bytes
                //ComputeHash, aplica o algoritmo do HMACSHA512 no array de bytes da senha, para ficar criptografado
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));


            }
        }
        //método que retorna um true or false
        public bool VerificaSenha(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            //var hmac = new HMACSHA512(senhaSalt), vou gerar um hash de acordo com a senha que eu passei (senhaSalt)
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                //System.Text.Encoding.UTF8.GetBytes(senha), convertendo a string senha em um array de bytes
                //hmac.ComputeHash,calcula o hash da entrada fornecida, combina o hash do hmca(senhaSalt) com a  "senha" e gera um hash único para essa combinação
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                //SequenceEqual, compara dois array de bytes para ver se são iguais, se forem retornam true
                return computedHash.SequenceEqual(senhaHash);
            }
        }
    }
}
