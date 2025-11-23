using Discord;
using Discord.WebSocket;
using MarcoPortefolioServer.Functions.v1.modules.server.Discord.Utils;
using MarcoPortefolioServer.Functions.v1.modules.server.Discord; // <- IMPORTANTE
using Microsoft.Extensions.Configuration;
using System;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord
{
    public class DiscordBot
    {
        private readonly DiscordSocketClient _client;
        private readonly string _token;
        private readonly IConfiguration _config;

        public DiscordBot(string token)
        {
            _token = token;

            // Carregar config (appsettings.json)
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true
            });

            _client.Log += LogAsync;
            _client.Ready += OnReady;
            _client.SlashCommandExecuted += HandleSlashCommand;
        }

        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine("[Discord] " + msg.ToString());
            return Task.CompletedTask;
        }

        // ===============================================
        // REGISTER SLASH COMMANDS
        // ===============================================
        private RegisterCommand _registry;

        private async Task OnReady()
        {
            Console.WriteLine($"[Discord] Ligado como {_client.CurrentUser.Username}");

            // Registrar comandos
            _registry = new RegisterCommand(_client);
            await _registry.RegisterAll();

            // ===============================================
            // INICIAR STATUS UPDATE SERVICE
            // ===============================================
            Console.WriteLine("[Discord] StatusUpdateService iniciado...");

            var statusService = new StatusUpdateService(_client, _config);
            _ = statusService.StartAsync(); // fire & forget
        }

        private async Task HandleSlashCommand(SocketSlashCommand cmd)
        {
            var command = _registry.Commands.FirstOrDefault(c => c.Name == cmd.CommandName);
            if (command != null)
                await command.Execute(cmd);
        }
    }
}