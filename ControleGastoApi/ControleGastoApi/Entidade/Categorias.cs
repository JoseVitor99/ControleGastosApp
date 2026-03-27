using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    // Utilização de enum para definir valores fixos para tipagem
    public enum FinalidadeCategoria
    {
        Receita = 1,
        Despesa = 2,
        Ambas = 3
    }
    public class Categoria
    {
        public int Id { get; set; }

        [MaxLength(400)]
        [Required]
        public string? Descricao { get; set; }
        public FinalidadeCategoria Finalidade { get; set; }
    }

}