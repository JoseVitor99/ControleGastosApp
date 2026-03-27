namespace ControleGastoApi.DTO
{
    public class TransacaoDTO
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Pessoa { get; set; } = string.Empty;
    }
}
