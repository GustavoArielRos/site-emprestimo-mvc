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
    }
}
