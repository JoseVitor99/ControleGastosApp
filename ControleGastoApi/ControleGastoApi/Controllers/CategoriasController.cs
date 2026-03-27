using ControleGastoApi.Data;
using ControleGastoApi.Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _context.Categorias.ToListAsync();
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return Ok(categoria);
    }

    [HttpGet("totais")]
    public async Task<IActionResult> GetTotais()
    {
        var totais = await _context.Transacoes.GroupBy(t => t.CategoriaId).Select(g => new
        {
            CategoriaId = g.Key,
            totalReceitas = g.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => (decimal?)t.Valor) ?? 0,

            totalDespesas = g.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => (decimal?)t.Valor) ?? 0,
        }).ToListAsync();

        var categorias = await _context.Categorias.ToListAsync();

        var resultado = categorias.Select(c =>
        {
            var total = totais.FirstOrDefault(t => t.CategoriaId == c.Id);

            var receitas = total?.totalReceitas ?? 0;
            var despesas = total?.totalDespesas ?? 0;

            return new
            {
                descricao = c.Descricao,
                totalReceitas = receitas,
                totalDespesas = despesas,
                saldo = receitas - despesas
            };
        });

        return Ok(resultado);
    }
}