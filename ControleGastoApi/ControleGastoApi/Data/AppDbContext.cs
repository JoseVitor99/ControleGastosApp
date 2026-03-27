using ControleGastoApi.Entidade;
using Microsoft.EntityFrameworkCore;

namespace ControleGastoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacoes> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transacoes>()
                .HasOne(t => t.Categoria)
                .WithMany()
                .HasForeignKey(t => t.CategoriaId);

            modelBuilder.Entity<Transacoes>()
                .HasOne(t => t.Pessoa)
                .WithMany(p => p.Transacoes)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}