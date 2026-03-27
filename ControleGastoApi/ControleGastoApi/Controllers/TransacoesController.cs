using ControleGastoApi.Entidade;
using ControleGastoApi.Services;
using ControleGastoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _service;
    private readonly AppDbContext _context;

    public TransacoesController(TransacaoService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transacoes transacao)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _service.Create(transacao);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            //return BadRequest(ex.ToString());
        }
    }

    //[HttpGet]
    //public async Task<IActionResult> Get()
    //{
    //    var transacoes = await _context.Transacoes
    //        .Include(t => t.Pessoa)
    //        .Include(t => t.Categoria)
    //        .ToListAsync();

    //    return Ok(transacoes);
    //}

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }
}