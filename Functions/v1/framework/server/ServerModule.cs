namespace MarcoPortefolioServer.Functions.v1.framework.server
{
    public class ServerModule
    {
        public static void StartServer()
        {
            Console.WriteLine("Iniciando tarefa do servidor...");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Servidor está processando a iteração {i}");
                Task.Delay(1000).Wait();
            }
            Console.WriteLine("Tarefa do servidor concluída.");
        }

        public static void RunServerModule()
        {
            ModuleExecutor.CreateServerThreadInternal(StartServer);
        }
    }
}