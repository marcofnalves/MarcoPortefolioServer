using MarcoPortefolioServer.Models.fivem;
using MarcoPortefolioServer.Functions.v1.framework.client; // Importando a função CreateClientThreadInternal

namespace MarcoPortefolioServer.Functions.v1.Discord.client
{
    public static class Client
    {
        public static void StartClientModule()
        {
            CreateClientThreadInternal(() =>
            {
                Console.WriteLine("Iniciando Client Discord...");
                TriggerServerEventInternal("DiscordClientReady", Array.Empty<object>());
            });
        }
    }
}