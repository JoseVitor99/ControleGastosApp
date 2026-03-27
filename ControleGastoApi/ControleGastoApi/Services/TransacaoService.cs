using ControleGastoApi.Data;
using ControleGastoApi.DTO;
using ControleGastoApi.Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastoApi.Services
{
    /// <summary>
    /// Serviço responsável pelas regras de negócio relacionadas às transações.
    ///
    /// Centraliza validações e garante a integridade dos dados antes de persistir.
    /// Atua como intermediário entre o Controller e o acesso ao banco de dados.
    /// </summary>

    public class TransacaoService
    {
        private readonly AppDbContext _context;

        public TransacaoService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria uma nova transação aplicando todas as regras de negócio.
        ///
        /// Regras implementadas:
        /// - O valor da transação deve ser maior que zero
        /// - A pessoa deve existir
        /// - Menores de idade não podem possuir receitas
        /// - A categoria deve existir
        /// - A categoria deve ser compatível com o tipo da transação
        /// </summary>

        public async Task<TransacaoDTO> Create(Transacoes transacao)
        {
            //Regra Valor Positivo
            if (transacao.Valor <= 0)
                throw new Exception("Valor deve ser maior que zero.");

            //Busca Pessoa
            var pessoa = await _context.Pessoas.FindAsync(transacao.PessoaId);
            if (pessoa == null)
                throw new Exception("Pessoa não encontrada.");

            //Menor de idade
            if (pessoa.Idade < 18 && transacao.Tipo != TipoTransacao.Despesa)
                throw new Exception("Menor de idade não pode ter receita.");

            //Busc aCategoria
            var categoria = await _context.Categorias.FindAsync(transacao.CategoriaId);
            if (categoria == null)
                throw new Exception("Categoria não encontrada.");

            //Compatibilidade
            if (transacao.Tipo == TipoTransacao.Despesa && categoria.Finalidade == FinalidadeCategoria.Receita
                || transacao.Tipo == TipoTransacao.Receita && categoria.Finalidade == FinalidadeCategoria.Despesa)
                throw new Exception("Categoria incompatível com o tipo.");


            // Persistência da transação
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            // Retorno em formato DTO (evita exposição direta da entidade)
            return new TransacaoDTO
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo.ToString(),
                Categoria = categoria.Descricao ?? "",
                Pessoa = pessoa.Nome ?? ""
            };
        }

        /// <summary>
        /// Retorna todas as transações cadastradas.
        ///
        /// Inclui dados relacionados (Pessoa e Categoria) e
        /// converte para DTO para padronizar a resposta da API.
        /// </summary>

        public async Task<List<TransacaoDTO>> GetAll()
        {
            var transacoes = await _context.Transacoes
                .Include(t => t.Pessoa)
                .Include(t => t.Categoria)
                .ToListAsync();

            return transacoes.Select(t => new TransacaoDTO
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Tipo = t.Tipo.ToString(),
                Categoria = t.Categoria?.Descricao ?? string.Empty,
                Pessoa = t.Pessoa?.Nome ?? string.Empty
            }).ToList();
        }
    }
}
