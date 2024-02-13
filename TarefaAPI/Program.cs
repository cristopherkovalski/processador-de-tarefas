
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;
using Repositorio;
using SOLID_Example.Interfaces;

namespace TarefaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IRepository<Tarefa>, TarefaMockRepository>();
            builder.Services.AddScoped<IGerenciadorTarefas, GerenciadorTarefas>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
       
       
    }
}
