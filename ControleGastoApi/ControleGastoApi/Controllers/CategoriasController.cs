using ControleGastoApi.Data;
using ControleGastoApi.Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller responsável pelo gerenciamento de categorias.
///
/// Permite:
/// - Listar categorias cadastradas
/// - Criar novas categorias
/// - Consultar totais de receitas, despesas e saldo por categoria
/// </summary>

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _context.Categorias.ToListAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// Realiza validação do modelo antes de persistir.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return Ok(categoria);
    }

    /// <summary>
    /// Retorna o total de receitas, despesas e saldo por categoria.
    ///
    /// Estratégia utilizada:
    /// 1- Agrupa as transações por CategoriaId
    /// 2- Calcula separadamente receitas e despesas por grupo
    /// 3- Junta os resultados com todas as categorias cadastradas (inclusive aquelas sem transações)
    /// 4- Calcula o saldo final (Receitas - Despesas)
    /// </summary>
    [HttpGet("totais")]
    public async Task<IActionResult> GetTotais()
    {
        // Agrupamento das transações por categoria
        var totais = await _context.Transacoes.GroupBy(t => t.CategoriaId).Select(g => new
        {
            CategoriaId = g.Key,
            // Soma das receitas por categoria
            totalReceitas = g.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => (decimal?)t.Valor) ?? 0,

            // Soma das despesas por categoria
            totalDespesas = g.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => (decimal?)t.Valor) ?? 0,
        }).ToListAsync();

        // Lista completa de categorias
        var categorias = await _context.Categorias.ToListAsync();

        // Combinação dos dados de categorias com seus respectivos totais
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

                // Cálculo do saldo final
                saldo = receitas - despesas
            };
        });

        return Ok(resultado);
    }
}