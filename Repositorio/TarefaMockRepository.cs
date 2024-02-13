using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace Repositorio
{
    public class TarefaMockRepository : IRepository<Tarefa>
    {
        private static List<Tarefa> Tarefas;

        public TarefaMockRepository()
        {
            Tarefas = CriarTarefasMock();
        }

        public void Add(Tarefa entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.SubtarefasPendentes = CriarSubtarefasMock();
            Tarefas.Add(entity);
        }

        public IEnumerable<Tarefa> GetAll()
        {
            return Tarefas;
        }

        public Tarefa? GetById(int id)
        {
            return Tarefas.FirstOrDefault(t => t.Id == id);
        }


        public void Update(Tarefa entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            
            Tarefa tarefaExistente = Tarefas.FirstOrDefault(t => t.Id == entity.Id);

            if (tarefaExistente != null)
            {
                
                tarefaExistente.Estado = entity.Estado;
                tarefaExistente.IniciadaEm = entity.IniciadaEm;
                tarefaExistente.EncerradaEm = entity.EncerradaEm;
                tarefaExistente.SubtarefasPendentes = entity.SubtarefasPendentes;
                tarefaExistente.SubtarefasExecutadas = entity.SubtarefasExecutadas;
            }
            else
            {
                throw new ArgumentNullException("Tarefa não existe!");
            }
        }

        public void Cancelar(int id)
        {
                if (id == null)
                {
                    throw new ArgumentNullException("Valor nulo");
                }

                Tarefa tarefaExistente = Tarefas.FirstOrDefault(t => t.Id == id);
                if (tarefaExistente.Estado != EstadoTarefa.EmPausa || tarefaExistente.Estado != EstadoTarefa.Concluida )
                {
                    tarefaExistente.Estado = EstadoTarefa.Cancelada;
                }
                else
                {
                    throw new InvalidOperationException($"Não foi possível cancelar a Tarefa {tarefaExistente.Id} pois o seu estado atual é {tarefaExistente.Estado}.");
                }

        }

        private List<Tarefa> CriarTarefasMock()
        {
            Random random = new Random();
            List<Tarefa> tarefasMock = new List<Tarefa>();

            for (int i = 1; i <= 10; i++)
            {
                Tarefa tarefa = new Tarefa(i, EstadoTarefa.Criada, null, null, CriarSubtarefasMock(),null);
                tarefasMock.Add(tarefa);
            }

            return tarefasMock;
        }

        private List<Subtarefa> CriarSubtarefasMock()
        {
            Random random = new Random();
            List<Subtarefa> subtarefasMock = new List<Subtarefa>();

            int quantidadeSubtarefas = random.Next(0, 11);

            for (int i = 1; i <= quantidadeSubtarefas; i++)
            {
                Subtarefa subtarefa = new Subtarefa
                {
                    Duracao = TimeSpan.FromSeconds(random.Next(1, 11)) 
                };

                subtarefasMock.Add(subtarefa);
            }

            return subtarefasMock;
        }
    }
}
