using ControleGastoApi.Data;
using ControleGastoApi.Entidade;
using ControleGastoApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/pessoas")]
public class PessoaController : ControllerBase
{
    private readonly AppDbContext _context;

    public PessoaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pessoas = await _context.Pessoas.ToListAsync();
        return Ok(pessoas);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Pessoa pessoa)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return Ok(pessoa);
    }

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

    [HttpGet("totais")]
    public async Task<IActionResult> GetTotais()
    {
        var pessoas = await _context.Pessoas.Include(p => p.Transacoes).ToListAsync();

        var resultado = pessoas.Select(p => new
        {
            Nome = p.Nome,
            TotalReceitas = p.Transacoes?.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor) ?? 0,

            TotalDespesas = p.Transacoes?.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor) ?? 0
        })
        .Select(p => new
        {
            p.Nome,
            p.TotalReceitas,
            p.TotalDespesas,
            Saldo = p.TotalReceitas - p.TotalDespesas
        });

        return Ok(resultado);
    }
}