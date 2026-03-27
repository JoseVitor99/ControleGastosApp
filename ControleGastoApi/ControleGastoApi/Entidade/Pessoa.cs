using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    /// <summary>
    /// Representa uma pessoa cadastrada no sistema.
    ///
    /// Uma pessoa pode possuir várias transações associadas.
    /// </summary>

    public class Pessoa
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Nome { get; set; }
        public int Idade { get; set; }

        /// <summary>
        /// Lista de transações vinculadas à pessoa.
        /// Utilizada para relacionamento com a entidade de transações.
        /// </summary>
        public List<Transacoes>? Transacoes { get; set; }
    }
}