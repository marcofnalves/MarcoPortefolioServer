using System;
using System.Threading.Tasks;

namespace MarcoPortefolioServer.Functions.v1.framework
{
    public static class ModuleExecutor
    {
        private static bool isServerStarted = false;

        // Criar e executar a thread do cliente
        public static void CreateClientThreadInternal(Action clientTask)
        {
            // Espera ativa até que o servidor esteja iniciado
            while (!isServerStarted)
            {
                // Espera 500ms antes de verificar novamente, para evitar uma espera ativa 100% do tempo
                Task.Delay(500).Wait();
            }

            // Quando o servidor estiver iniciado, executa a tarefa do cliente
            Task.Run(() =>
            {
                try
                {
                    clientTask.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no cliente: {ex.Message}");
                }
            });
        }

        // Criar e executar a thread do servidor
        public static void CreateServerThreadInternal(Action serverTask)
        {
            Task.Run(() =>
            {
                try
                {
                    serverTask.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no servidor: {ex.Message}");
                }
            });
        }

        // Método para sinalizar que o servidor foi iniciado
        public static void MarkServerAsStarted()
        {
            isServerStarted = true;
            Console.WriteLine("Servidor iniciado, o cliente pode agora iniciar.");
        }
    }
}