using Discord.WebSocket;
using MarcoPortefolioServer.Functions.v1.modules.server.Discord.Commands;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord.Utils
{
    public class RegisterCommand
    {
        private readonly DiscordSocketClient _client;

        // Lista de comandos suportados
        public readonly List<ICommand> Commands = new();

        public RegisterCommand(DiscordSocketClient client)
        {
            _client = client;

            Commands.Add(new SayCommand());
        }

        // Registar todos os comandos nos guilds
        public async Task RegisterAll()
        {
            foreach (var guild in _client.Guilds)
            {
                foreach (var cmd in Commands)
                {
                    await guild.CreateApplicationCommandAsync(cmd.Build().Build());
                    Console.WriteLine($"[Discord] Comando /{cmd.Name} registado em {guild.Name}");
                }
            }
        }
    }
}