using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    /// <summary>
    /// Define os tipos possíveis de transação.
    /// Utilizado para classificar se a transação é uma receita ou despesa.
    /// </summary>
    public enum TipoTransacao
    {
        Despesa = 1,
        Receita = 2
    }

    /// <summary>
    /// Representa uma transação financeira no sistema.
    /// Cada transação está vinculada a uma pessoa e a uma categoria, permitindo a organização e controle financeiro.
    /// </summary>
    public class Transacoes
    {
        public int Id { get; set; }

        [MaxLength(400)]
        [Required]
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public int CategoriaId { get; set; }

        /// <summary>
        /// Objeto de navegação para a categoria.
        /// </summary>
        public Categoria? Categoria { get; set; }
        public int PessoaId { get; set; }

        /// <summary>
        /// Objeto de navegação para a pessoa.
        /// </summary>
        public Pessoa? Pessoa { get; set; }
    }
}