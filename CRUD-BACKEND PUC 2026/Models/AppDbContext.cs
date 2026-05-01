using Microsoft.EntityFrameworkCore;

namespace CrudApp.Models
{
    /// <summary>
    /// Contexto do Entity Framework Core.
    /// É a "ponte" entre as classes C# e as tabelas do banco de dados.
    /// </summary>
    public class AppDbContext : DbContext
    {
        // O construtor recebe as opções (connection string, provider) via injeção de dependência
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cada DbSet representa uma tabela no banco
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed de dados iniciais – categorias padrão
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Eletrônicos",  Descricao = "Dispositivos eletrônicos em geral" },
                new Categoria { Id = 2, Nome = "Vestuário",    Descricao = "Roupas e acessórios" },
                new Categoria { Id = 3, Nome = "Alimentação",  Descricao = "Produtos alimentícios" }
            );
        }
    }
}
