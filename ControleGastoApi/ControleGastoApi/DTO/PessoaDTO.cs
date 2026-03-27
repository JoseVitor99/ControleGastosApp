namespace ControleGastoApi.DTO
{
    /// <summary>
    /// DTO utilizado para representar o resumo financeiro de uma pessoa.
    ///
    /// Contém os totais de receitas, despesas e o saldo calculado,
    /// sendo utilizado na consulta de totais por pessoa.
    /// </summary>
    public class PessoaDTO
    {
        public string? Nome { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }

        /// <summary>
        /// Saldo calculado automaticamente com base nas receitas e despesas.
        /// </summary>
        public decimal Saldo => TotalReceitas - TotalDespesas;
    }
}
