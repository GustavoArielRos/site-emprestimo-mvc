using EmprestimoLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //criando a tabela pro sql
        public DbSet<EmprestimosModel> Emprestimos { get; set; }

        //criando a tabela que possui as  informações do usuário
        public DbSet<UsuarioModel> Usuarios { get; set; }

    }
}
