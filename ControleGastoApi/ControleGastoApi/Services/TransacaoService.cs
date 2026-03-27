using ControleGastoApi.Data;
using ControleGastoApi.DTO;
using ControleGastoApi.Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastoApi.Services
{
    public class TransacaoService
    {
        private readonly AppDbContext _context;

        public TransacaoService(AppDbContext context)
        {
            _context = context;
        }

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


            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

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
                Categoria = t.Categoria != null ? t.Categoria.Descricao : "",
                Pessoa = t.Pessoa != null ? t.Pessoa.Nome : ""
            }).ToList();
        }
    }
}
