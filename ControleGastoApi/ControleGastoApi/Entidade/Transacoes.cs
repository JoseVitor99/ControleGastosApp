using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    public enum TipoTransacao
    {
        Despesa = 1,
        Receita = 2
    }
    public class Transacoes
    {
        public int Id { get; set; }

        [MaxLength(400)]
        [Required]
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public int PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
    }
}