using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;
using Repositorio;
using SOLID_Example.Interfaces;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            var serviceProvider = ConfigureServiceProvider();
           


        }
        private static IServiceProvider ConfigureServiceProvider()
        {
            string connectionString = "Data Source=database.db";

            IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddScoped(_ => configuration);
            services.AddScoped<IRepository<Tarefa>, TarefaMockRepository>();
            services.AddScoped<IGerenciadorTarefas, GerenciadorTarefas>(serviceProvider =>
            {
                var repository = serviceProvider.GetService<IRepository<Tarefa>>();
                return new GerenciadorTarefas(repository, serviceProvider);
            });

            return services.BuildServiceProvider(); ;
        }
    }
}
