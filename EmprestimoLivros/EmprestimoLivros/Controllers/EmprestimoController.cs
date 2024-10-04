using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ApplicationDbContext _db;
        readonly private ISessaoInterface _sessaoInterface;

        public EmprestimoController(ApplicationDbContext db, ISessaoInterface sessaoInterface)
        {
            _db = db;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {

            var usuario = _sessaoInterface.BuscarSessao();
            if(usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
        }


        [HttpGet]
        public IActionResult Cadastrar()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Editar(int ? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if(emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if(emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);


        }

        public IActionResult Exportar()
        {   
            //chamo o método "GetDados"  pegando os dados que serão exportados
            var dados = GetDados();

            //XLWorkbook representa um arquivo em excel , using serve que o recuro seja liberado corretamento após o uso
            using (XLWorkbook workBook = new XLWorkbook())
            {   
                //adicionando uma nova planilha no objeto criado usando os dados do "GetDados()" e nomeia de "Dados Empréstimo
                workBook.AddWorksheet(dados, "Dados Empréstimo");

                //grava os dadso em memória
                using (MemoryStream ms = new MemoryStream())
                {   
                    //salva o arquivo excel no ms 
                    workBook.SaveAs(ms);

                    //retorna um arquivo http
                    //ms.ToArray(), conteudo do arquivo em forma de array
                    //application/vnd.openxmlformats-officedocument.spredsheetml.sheet, identificando o tipo de arquivo, no caso é um openxml(formato padrão do excel)
                    //"Emprestimo.xls", nome do arquivo
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet","Emprestimo.xls");
                }


            }

            return Ok();
        }

        //retorna um dataTable(estrutura de dados que armazena dados em forma de tabela)
        private DataTable GetDados()
        {   
            //Cria um objeto do tipo DataTable
            DataTable dataTable = new DataTable();

            //adicionando no atributo o nome
            dataTable.TableName = "Dados empréstimos";

            //adicionando as colunas ao dataTable para armazenar os dados dos empréstimos(defininindo o nome da coluna e seu tipo)
            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("Data empréstimo", typeof(DateTime));

            //obtendo os dados do banco de dados como lista
            var dados = _db.Emprestimos.ToList();

            //se tiver dados
            if(dados.Count > 0 )
            {   
                //para cada item na lista "dados" eu adiciona uma nova linha ao dataTable
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.dataUltimaAtualizacao);
                });
            }

            //retorna o dataTable
            return dataTable;
        }


        //método que ajuda a criar algo 
        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimo)
        {   
            //se for válido
            if(ModelState.IsValid)
            {
                emprestimo.dataUltimaAtualizacao = DateTime.Now;


                _db.Emprestimos.Add(emprestimo);
                _db.SaveChanges();

                //é um armazenamento temporário
                //armazena dados entre duas requisições http teporarioamente
                //é usado para feedback como mensagem de sucesso ou erro,após uma ação que redirecona para outra página
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }

            return View();
        }


        [HttpPost]

        public IActionResult Editar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {

                var emprestimoDB = _db.Emprestimos.Find(emprestimo.Id);

                emprestimoDB.Fornecedor = emprestimo.Fornecedor;
                emprestimoDB.Recebedor = emprestimo.Recebedor;
                emprestimoDB.LivroEmprestado = emprestimo.LivroEmprestado;


                _db.Emprestimos.Update(emprestimoDB);
                _db.SaveChanges();

                //é um armazenamento temporário
                //armazena dados entre duas requisições http teporarioamente
                //é usado para feedback como mensagem de sucesso ou erro,após uma ação que redirecona para outra página
                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";

                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Algum erro ocorreu ao realizar a edição!";

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimo)
        {
            if(emprestimo == null)
            {
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimo);

            _db.SaveChanges();


            //é um armazenamento temporário
            //armazena dados entre duas requisições http teporarioamente
            //é usado para feedback como mensagem de sucesso ou erro,após uma ação que redirecona para outra página
            TempData["MensagemSucesso"] = "Remoção realizada com sucesso!";

            return RedirectToAction("Index");

        }

    }
}
