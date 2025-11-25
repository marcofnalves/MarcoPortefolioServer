using Discord.WebSocket;
using MarcoPortefolioServer.Functions.v1.Discord.server.Commands;
using MarcoPortefolioServer.Functions.v1.lib.server;

namespace MarcoPortefolioServer.Functions.v1.Discord.server.Utils
{
    public class RegisterCommand
    {
        private readonly DiscordSocketClient _DiscordClient;

        // Lista de comandos suportados
        public readonly List<ICommand> Commands = new();

        public RegisterCommand(DiscordSocketClient DiscordClient)
        {
            _DiscordClient = DiscordClient;

            Commands.Add(new SayCommand());
            Commands.Add(new WhitelistCommand());
        }

        // Registar todos os comandos nos guilds
        public async Task RegisterAll()
        {
            foreach (var guild in _DiscordClient.Guilds)
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