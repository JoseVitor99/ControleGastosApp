using ControleGastoApi.Entidade;
using ControleGastoApi.Services;
using ControleGastoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller responsável pelo gerenciamento de transações.
///
/// Atua como camada de entrada da API, delegando a lógica de negócio
/// ao serviço de transações.
///
/// Permite:
/// - Criar novas transações
/// - Listar transações cadastradas
/// </summary>

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

    /// <summary>
    /// Cria uma nova transação.
    ///
    /// A lógica de validação e persistência é delegada ao TransacaoService,
    /// garantindo melhor organização e separação de responsabilidades.
    /// </summary>
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
        }
    }

    /// <summary>
    /// Retorna todas as transações cadastradas.
    ///
    /// A obtenção dos dados é realizada via serviço, que pode incluir
    /// regras adicionais como projeções (DTO) ou inclusão de relacionamentos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }
}