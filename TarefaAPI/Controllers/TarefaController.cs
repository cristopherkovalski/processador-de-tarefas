using Microsoft.AspNetCore.Mvc;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;

namespace TarefaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly IGerenciadorTarefas _gerenciadorTarefas;

        public TarefaController(IGerenciadorTarefas gerenciadorTarefas)
        {
            _gerenciadorTarefas = gerenciadorTarefas;
        }

        [HttpPost("criar")]
        public async Task<ActionResult<Tarefa>> CriarTarefa()
        {
            var tarefa = await _gerenciadorTarefas.Criar();
            return Ok(tarefa);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> ConsultarTarefa(int id)
        {
            var tarefa = await _gerenciadorTarefas.Consultar(id);
            if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<ActionResult> CancelarTarefa(int id)
        {
            await _gerenciadorTarefas.Cancelar(id);
            return NoContent();
        }

        [HttpGet("ativas")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ListarTarefasAtivas()
        {
            var tarefas = await _gerenciadorTarefas.ListarAtivas();
            return Ok(tarefas);
        }

        [HttpGet("inativas")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> ListarTarefasInativas()
        {
            var tarefas = await _gerenciadorTarefas.ListarInativas();
            return Ok(tarefas);
        }
    }
}
