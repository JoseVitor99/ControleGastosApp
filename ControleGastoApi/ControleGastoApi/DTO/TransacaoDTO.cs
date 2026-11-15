namespace ControleGastoApi.DTO
{
    /// <summary>
    /// Objeto de transferência de dados utilizado para retornar informações de transações.
    /// Evita expor diretamente a entidade do banco e permite formatar os dados
    /// de forma mais adequada para consumo no frontend.
    /// </summary>
    public class TransacaoDTO
    {
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        /// <summary>
        /// Tipo da transação (Receita ou Despesa) em formato textual.
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Nome da categoria associada à transação.
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Nome da pessoa associada à transação.
        /// </summary>
        public string Pessoa { get; set; } = string.Empty;
    }
}
