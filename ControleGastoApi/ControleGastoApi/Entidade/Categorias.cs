using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    /// <summary>
    /// Utilização de enum para definir valores fixos para tipagem
    /// </summary>
    public enum FinalidadeCategoria
    {
        Receita = 1,
        Despesa = 2,
        Ambas = 3
    }

    /// <summary>
    /// Representa uma categoria utilizada para classificar transações.
    /// Cada categoria possui uma finalidade que define se pode ser
    /// usada para receitas, despesas ou ambos.
    /// </summary>
    public class Categoria
    {
        public int Id { get; set; }

        [MaxLength(400)]
        [Required]
        public string? Descricao { get; set; }
        public FinalidadeCategoria Finalidade { get; set; }
    }

}