namespace EmprestimoLivros.Models
{
    public class ResponseModal<T>
    {   
        //T significa que pode ser de qualquer tipo, no caso indicando que pode ser qualquer tipo de modelo
        public T? Dados { get; set; }

        public string Mensagem { get; set; } = string.Empty;

        public bool Status { get; set; } = true;


    }
}
