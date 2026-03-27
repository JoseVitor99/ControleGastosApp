namespace ControleGastoApi.DTO
{
    public class PessoaDTO
    {
        public string? Nome { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo => TotalReceitas - TotalDespesas;
    }
}
