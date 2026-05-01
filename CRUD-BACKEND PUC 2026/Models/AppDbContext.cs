using Microsoft.EntityFrameworkCore;

namespace CrudApp.Models
{
    /// </summary>
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cada DbSet representa uma tabela no banco
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Eletrônicos",  Descricao = "Dispositivos eletrônicos em geral" },
                new Categoria { Id = 2, Nome = "Vestuário",    Descricao = "Roupas e acessórios" },
                new Categoria { Id = 3, Nome = "Alimentação",  Descricao = "Produtos alimentícios" }
            );
        }
    }
}
