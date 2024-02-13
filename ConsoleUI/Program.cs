using System;
using Microsoft.Extensions.DependencyInjection;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;
using Repositorio;
using SOLID_Example.Interfaces;

namespace SOLID_Example
{
    internal class Program
        {
            private static async Task Main(string[] args)
            {
                var repo = new TarefaMockRepository();
                var gerenciador = new GerenciadorTarefas(repo);
                var processador = new ProcessadorTarefas.Servicos.ProcessadorTarefas(gerenciador);

            await processador.Iniciar();


            _ = AtualizarProgresso(gerenciador);

            await ExibirMenuAsync(gerenciador);
            await processador.ExecutarTarefasAgendadas();
        }

        private static async Task ExibirMenuAsync(IGerenciadorTarefas gerenciador)
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("==== Menu ====");
                Console.WriteLine("1. Cancelar tarefa");
                Console.WriteLine("2. Consultar tarefa");
                Console.WriteLine("3. Criar tarefa");
                Console.WriteLine("4. Listar tarefas ativas");
                Console.WriteLine("5. Listar tarefas inativas");
                Console.WriteLine("6. Sair");

                Console.WriteLine();
                Console.WriteLine("Tarefas em andamento:");

                var tarefas = await gerenciador.ListarAtivas();
                MostrarProgressoTemporal(tarefas);

                Console.WriteLine("\nDigite o número da operação desejada:");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.WriteLine("Digite o ID da tarefa a ser cancelada:");
                        if (int.TryParse(Console.ReadLine(), out int idCancelar))
                        {
                            await gerenciador.Cancelar(idCancelar);
                            Console.WriteLine($"Tarefa {idCancelar} cancelada com sucesso.");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Digite o ID da tarefa a ser consultada:");
                        if (int.TryParse(Console.ReadLine(), out int idConsultar))
                        {
                            var tarefaConsultada = await gerenciador.Consultar(idConsultar);
                            Console.WriteLine($"Tarefa consultada: {tarefaConsultada.Id} {tarefaConsultada.Estado}");
                        }
                        break;
                    case "3":
                        var novaTarefa = await gerenciador.Criar();
                        Console.WriteLine($"Tarefa criada com ID: {novaTarefa.Id}");
                        break;
                    case "4":
                        var tarefasAtivas = await gerenciador.ListarAtivas();
                        foreach (var tarefa in tarefasAtivas)
                        {
                            Console.WriteLine($"ID: {tarefa.Id} - Estado: {tarefa.Estado}");
                        }
                        break;
                    case "5":
                        var tarefasInativas = await gerenciador.ListarInativas();
                        foreach (var tarefa in tarefasInativas)
                        {
                            Console.WriteLine($"ID: {tarefa.Id} - Estado: {tarefa.Estado}");
                        }
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

       
        private static async Task AtualizarProgresso(IGerenciadorTarefas gerenciador)
        {
            while (true)
            {
                await Task.Delay(2500); 

                Console.Clear();
                Console.WriteLine("==== Menu ====");
                Console.WriteLine("1. Cancelar tarefa");
                Console.WriteLine("2. Consultar tarefa");
                Console.WriteLine("3. Criar tarefa");
                Console.WriteLine("4. Listar tarefas ativas");
                Console.WriteLine("5. Listar tarefas inativas");
                Console.WriteLine("6. Sair");

                Console.WriteLine();
                Console.WriteLine("Tarefas em andamento:");

                var tarefas = await gerenciador.ListarAtivas();
                MostrarProgressoTemporal(tarefas);
            }
        }

        private static void MostrarProgressoTemporal(IEnumerable<Tarefa> tarefas)
        {
            foreach (var tarefa in tarefas)
            {
                string progresso = "";

                if (tarefa.Estado == EstadoTarefa.EmExecucao)
                {
                    var duracaoTotal = tarefa.SubtarefasPendentes.Sum(subtarefa => subtarefa.Duracao.TotalMilliseconds);
                    if (duracaoTotal > 0)
                    {
                        var tempoDecorrido = DateTime.Now - tarefa.IniciadaEm.GetValueOrDefault();
                        if (tempoDecorrido.TotalMilliseconds < duracaoTotal)
                        {
                            double porcentagemConcluida = (tempoDecorrido.TotalMilliseconds / duracaoTotal) * 100;
                            int progressoAtual = (int)Math.Round(porcentagemConcluida / 10);
                            progresso = "[" + new string('=', progressoAtual) + new string(' ', 10 - progressoAtual) + "]";
                        }
                        else
                        {
                            progresso = "[==========]";
                        }
                    }
                }
                Console.WriteLine($"ID: {tarefa.Id} - Estado: {tarefa.Estado} - Progresso: {progresso}");
            }
        }
    }
}



    
