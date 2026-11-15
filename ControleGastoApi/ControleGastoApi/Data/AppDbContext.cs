using ControleGastoApi.Entidade;
using Microsoft.EntityFrameworkCore;

namespace ControleGastoApi.Data
{
    /// <summary>
    /// Contexto principal de acesso ao banco de dados da aplicação.
    ///
    /// Responsável por:
    /// - Representar as tabelas através dos DbSet
    /// - Configurar os relacionamentos entre as entidades
    /// - Gerenciar a comunicação com o banco via Entity Framework
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Tabela de pessoas cadastradas.
        /// </summary>
        public DbSet<Pessoa> Pessoas { get; set; }

        /// <summary>
        /// Tabela de categorias.
        /// </summary>
        public DbSet<Categoria> Categorias { get; set; }

        /// <summary>
        /// Tabela de transações financeiras.
        /// </summary>
        public DbSet<Transacoes> Transacoes { get; set; }

        /// <summary>
        /// Configuração dos relacionamentos entre as entidades.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento entre Transações e Categoria
            // Uma transação possui uma categoria associada
            modelBuilder.Entity<Transacoes>()
                .HasOne(t => t.Categoria)
                .WithMany()
                .HasForeignKey(t => t.CategoriaId);

            // Relacionamento entre Transações e Pessoa
            // Uma pessoa pode possuir várias transações
            // Ao excluir uma pessoa, suas transações também são removidas
            modelBuilder.Entity<Transacoes>()
                .HasOne(t => t.Pessoa)
                .WithMany(p => p.Transacoes)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}