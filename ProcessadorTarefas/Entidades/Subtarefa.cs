namespace ProcessadorTarefas.Entidades
{
    public struct Subtarefa
    { 
        public TimeSpan Duracao { get; set; }
        public Subtarefa(TimeSpan duracao)
        {
            Duracao = duracao;
        }
    }

}
