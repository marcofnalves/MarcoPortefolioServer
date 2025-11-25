using Discord;
using Discord.WebSocket;
using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Models.fivem;
using MarcoPortefolioServer.Repository.v1.fivem;
using System.ComponentModel;
using HttpRequest = MarcoPortefolioServer.Functions.v1.lib.server.HttpRequest;

namespace MarcoPortefolioServer.Functions.v1.Discord.server.Commands
{
    public class WhitelistCommand : ICommand
    {
        private readonly Server _server;
        public string Name => "whitelist";
        public WhitelistCommand(){}

        public SlashCommandBuilder Build()
        {
            return new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("Whitelist on Server No Mans Land")
                .AddOption("license", ApplicationCommandOptionType.String, "license", isRequired: true);
        }
        public async Task Execute(SocketSlashCommand cmd)
        {
            var license = cmd.Data.Options.First().Value.ToString();
            WhitelistModel existing = _server.GetExistingWhitelist(license);
            _server.UpdateWhitelist(existing.license, existing.license2, existing.steam, existing.discord, 1, 0);
            PlayersModel? serverStatus = null;
            bool isOnline = true;

            try
            {
                serverStatus = await HttpRequest.GetAsync<PlayersModel>(
                    "http://26.186.223.111:30120/players.json",
                    log: false
                );

                if (serverStatus == null)
                    isOnline = false;
                TriggerServerEventInternal("setWhitelist", new { license = license, playerName = cmd.User.Username });
            }
            catch
            {
                isOnline = false;
            }
            await cmd.RespondAsync($"Whitelist {string.Empty}");
        }
    }

    public class PlayerModel
    {
        public string Name { get; set; }
    }

    public class PlayersModel
    {
        public List<PlayerModel> Players { get; set; }
    }
}
