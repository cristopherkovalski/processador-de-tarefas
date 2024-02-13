using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;

namespace ProcessadorTarefas.Servicos
{
    public class ProcessadorTarefas : IProcessadorTarefas
    {
        private IGerenciadorTarefas _gerenciadorTarefas;
        private int contadorTarefasAgendadas;
        private Queue<Tarefa> TarefasAgendadas;
        public ProcessadorTarefas(IGerenciadorTarefas gerenciador)
        {
            _gerenciadorTarefas = gerenciador;
            

        }
        public Task CancelarTarefa(int idTarefa)
        {
            throw new NotImplementedException();
        }

        public Task Encerrar()
        {
            throw new NotImplementedException();
        }

        public Task Iniciar()
        {
                AgendarTarefas();
            var tarefas = _gerenciadorTarefas.ListarAtivas().GetAwaiter().GetResult();
           
            foreach (var tarefa in tarefas.Where(t => t.Estado == EstadoTarefa.EmPausa || t.Estado == EstadoTarefa.Agendada))
                {
                    tarefa.Estado = EstadoTarefa.EmExecucao;
                    Task.Run(async () =>
                        {

                            await ProcessarTarefa(tarefa);
                            contadorTarefasAgendadas--;
                            AgendarTarefas();
                        });

               
                }
            

            return Task.CompletedTask;
        }
      
        private async Task ProcessarTarefa(Tarefa tarefa)
        {
            tarefa.IniciadaEm = DateTime.Now;
            tarefa.Estado = EstadoTarefa.EmExecucao;
            foreach (var subtarefa in tarefa.SubtarefasPendentes.ToList())
            {
                await Task.Delay(subtarefa.Duracao);
                tarefa.SubtarefasPendentes.Except(new[] { subtarefa });
                tarefa.SubtarefasExecutadas.Append(subtarefa);
            }
            tarefa.Estado = EstadoTarefa.Concluida;
            tarefa.EncerradaEm = DateTime.Now;
          

        }

        private async Task AgendarTarefas()
        {
            var tarefasAtivas = await _gerenciadorTarefas.ListarAtivas();

            foreach (var tarefa in tarefasAtivas.Where(t => t.Estado == EstadoTarefa.Criada || t.Estado == EstadoTarefa.EmPausa))
            {
                if (tarefa.Estado != EstadoTarefa.Agendada && contadorTarefasAgendadas < 5)
                {
                    tarefa.Estado = EstadoTarefa.Agendada;
                    contadorTarefasAgendadas++;
                }
            }
        }


        public async Task ExecutarTarefasAgendadas()
        {
         

            var tarefas = await _gerenciadorTarefas.ListarAtivas();
 
            foreach (var tarefa in tarefas.Where(t => t.Estado == EstadoTarefa.Agendada || t.Estado == EstadoTarefa.EmPausa))
            {
                tarefa.Estado = EstadoTarefa.EmExecucao;
                Task.Run(async () =>
                {

                    await ProcessarTarefa(tarefa);

                });
            }

            await Task.Delay(50000);
        
            
        }
    }

 }
 
 
 

