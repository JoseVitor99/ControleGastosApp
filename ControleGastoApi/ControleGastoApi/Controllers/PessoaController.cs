using ControleGastoApi.Data;
using ControleGastoApi.Entidade;
using ControleGastoApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller responsável pelo gerenciamento de pessoas.
///
/// Permite:
/// - Listar pessoas cadastradas
/// - Criar novas pessoas
/// - Atualizar dados existentes
/// - Excluir pessoas
/// - Consultar totais de receitas, despesas e saldo por pessoa
/// </summary>

[ApiController]
[Route("api/pessoas")]
public class PessoaController : ControllerBase
{
    private readonly AppDbContext _context;

    public PessoaController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pessoas = await _context.Pessoas.ToListAsync();
        return Ok(pessoas);
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// Valida o modelo antes de persistir no banco.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Pessoa pessoa)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return Ok(pessoa);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Pessoa pessoa)
    {
        var existente = await _context.Pessoas.FindAsync(id);
        if (existente == null)
            return NotFound("Pessoa não encontrada.");

        existente.Nome = pessoa.Nome;
        existente.Idade = pessoa.Idade;

        await _context.SaveChangesAsync();
        return Ok(existente);
    }

    /// <summary>
    /// Remove uma pessoa pelo identificador.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null)
            return NotFound("Pessoa não encontrada.");

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Retorna o total de receitas, despesas e saldo de cada pessoa.
    ///
    /// Para cada pessoa:
    /// - Soma valores das transações do tipo Receita
    /// - Soma valores das transações do tipo Despesa
    /// - Calcula o saldo (Receitas - Despesas)
    /// </summary>
    [HttpGet("totais")]
    public async Task<IActionResult> GetTotais()
    {
        var pessoas = await _context.Pessoas.Include(p => p.Transacoes).ToListAsync();

        var resultado = pessoas.Select(p => new
        {
            Nome = p.Nome,

            // Soma das transações classificadas como receita
            TotalReceitas = p.Transacoes?.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor) ?? 0,

            // Soma das transações classificadas como despesa
            TotalDespesas = p.Transacoes?.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor) ?? 0
        })
        .Select(p => new
        {
            p.Nome,
            p.TotalReceitas,
            p.TotalDespesas,

            // Cálculo do saldo final
            Saldo = p.TotalReceitas - p.TotalDespesas
        });

        return Ok(resultado);
    }
}