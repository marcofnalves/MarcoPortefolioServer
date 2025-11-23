using Discord;
using Discord.WebSocket;
using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Functions.v1.modules.server.Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord
{
    public class StatusUpdateService
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        private readonly ulong _channelId;
        private readonly ulong _guildId;

        private ulong _messageId;

        public StatusUpdateService(DiscordSocketClient client, IConfiguration config)
        {
            _client = client;
            _config = config;

            _channelId = ulong.Parse(_config["Discord:StatusChannelId"]!);
            _guildId = ulong.Parse(_config["Discord:GuildId"]!);
            _messageId = ulong.Parse(_config["Discord:StatusMessageId"] ?? "0");
        }

        public async Task StartAsync()
        {
            // ciclo infinito de atualização
            while (true)
            {
                try
                {
                    await UpdateStatusAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[StatusUpdateService] Erro: {ex}");
                }

                await Task.Delay(30000); // Atualizar a cada 30 segundos
            }
        }

        private async Task UpdateStatusAsync()
        {
            var guild = _client.GetGuild(_guildId);
            var channel = guild?.GetTextChannel(_channelId);

            if (channel == null)
            {
                Console.WriteLine("[StatusUpdateService] ⚠ Canal inválido.");
                return;
            }

            ServerStatusModel? serverStatus = null;
            bool isOnline = true;

            try
            {
                serverStatus = await HttpRequest.GetAsync<ServerStatusModel>(
                    "http://26.186.223.111:30120/dynamic.json",
                    log: false
                );

                if (serverStatus == null)
                    isOnline = false;
            }
            catch
            {
                isOnline = false;
            }

            // Construir embed
            var embed = new EmbedBuilder()
                .WithTitle("No Mans Land — Status do Servidor")
                .WithColor(isOnline ? new Color(0, 255, 120) : new Color(255, 0, 0))
                .WithTimestamp(DateTimeOffset.UtcNow)
                .WithFooter(footer =>
                {
                    footer.Text = "Atualizado automaticamente";
                    footer.IconUrl = "https://i.imgur.com/klZ3NNi.png";
                });

            if (isOnline && serverStatus != null)
            {
                embed
                    .AddField("📡 Status", "🟢 **Online**", inline: true)
                    .AddField("👥 Jogadores", $"{serverStatus.clients} / {serverStatus.sv_maxclients}", inline: true)
                    .AddField("🖥️ Nome", serverStatus.hostname, inline: false)
                    .AddField("🌍 Mapa", serverStatus.mapname, inline: true)
                    .AddField("🕹️ Gamemode", serverStatus.gametype, inline: true);
            }
            else
            {
                embed.AddField("📡 Status", "🔴 **Offline**", inline: false);
            }

            // Criar mensagem nova se não existir
            if (_messageId == 0)
            {
                var sent = await channel.SendMessageAsync(embed: embed.Build());
                _messageId = sent.Id;

                SaveMessageId(_messageId);
                return;
            }

            // Editar mensagem existente
            var msg = await channel.GetMessageAsync(_messageId) as IUserMessage;
            if (msg != null)
            {
                await msg.ModifyAsync(m => m.Embed = embed.Build());
            }
        }

        // Guarda ID da mensagem no appsettings.json
        private void SaveMessageId(ulong id)
        {
            string path = "appsettings.json";
            var json = File.ReadAllText(path);

            dynamic data = Newtonsoft.Json.Linq.JObject.Parse(json);
            data["Discord"]["StatusMessageId"] = id.ToString();

            File.WriteAllText(path, data.ToString());
        }
    }
    public class ServerStatusModel { 
        public string hostname { get; set; } = ""; 
        public int clients { get; set; } 
        public int sv_maxclients { get; set; } 
        public string gametype { get; set; } = ""; 
        public string mapname { get; set; } = ""; 
    }
}