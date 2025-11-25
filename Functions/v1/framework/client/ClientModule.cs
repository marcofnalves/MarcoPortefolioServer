namespace MarcoPortefolioServer.Functions.v1.framework.client
{
    public class ClientModule
    {
        public static void StartClient()
        {
            Console.WriteLine("Iniciando tarefa do cliente...");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Cliente está processando a iteração {i}");
                Task.Delay(1000).Wait();
            }

            Console.WriteLine("Tarefa do cliente concluída.");
        }

        public static void RunClientModule()
        {
            ModuleExecutor.CreateClientThreadInternal(StartClient);
        }
    }
}