using System.ComponentModel.DataAnnotations;

namespace ControleGastoApi.Entidade
{
    public class Pessoa
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public List<Transacoes>? Transacoes { get; set; }
    }
}