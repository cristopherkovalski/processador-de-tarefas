using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    public class GerenciadorTarefas : IGerenciadorTarefas
    {
        private IRepository<Tarefa> _tarefaRepository;
        private readonly IServiceProvider _serviceProvider;

        public GerenciadorTarefas(IRepository<Tarefa> tarefaRepo, IServiceProvider serviceProvider)
        {
            _tarefaRepository = tarefaRepo;
            _serviceProvider = serviceProvider;
        }
        public GerenciadorTarefas(IRepository<Tarefa> tarefaRepo)
        {
            _tarefaRepository = tarefaRepo;

        }

        public async Task Cancelar(int idTarefa)
        {
            _tarefaRepository.Cancelar(idTarefa);
        }

        public async Task<Tarefa> Consultar(int idTarefa)
        {
            return await Task.FromResult(_tarefaRepository.GetById(idTarefa));
        }

        public async Task<Tarefa> Criar()
        {
            Tarefa ultimaTarefa = _tarefaRepository.GetAll().MaxBy(tarefa => tarefa.Id);
            Tarefa tarefa = new Tarefa(ultimaTarefa.Id + 1, EstadoTarefa.Criada, null, null, null, null);
            _tarefaRepository.Add(tarefa);
            return await Task.FromResult(tarefa);
        }

        public async Task<IEnumerable<Tarefa>> ListarAtivas()
        {

            return await Task.FromResult(_tarefaRepository.GetAll().Where(t => EstadoTarefa.Cancelada != t.Estado && EstadoTarefa.Concluida != t.Estado));
        }

        public async Task<IEnumerable<Tarefa>> ListarInativas()
        {
            return await Task.FromResult(_tarefaRepository.GetAll().Where(t => EstadoTarefa.Cancelada == t.Estado || EstadoTarefa.Concluida == t.Estado));
        }
    }
}
