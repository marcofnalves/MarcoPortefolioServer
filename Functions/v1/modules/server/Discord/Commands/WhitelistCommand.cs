using Discord;
using Discord.WebSocket;
using MarcoPortefolioServer.Repository.v1.fivem;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord.Commands
{
    public class WhitelistCommand : ICommand
    {
        public string Name => "whitelist";

        public SlashCommandBuilder Build()
        {
            return new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("Whitelist on Server No Mans Land")
                .AddOption("license", ApplicationCommandOptionType.String, "license", isRequired: true);
        }

        public async Task Execute(SocketSlashCommand cmd)
        {
            var msg = cmd.Data.Options.First().Value.ToString();
            NewWhitelistRepository repo = new NewWhitelistRepository();
        }
    }
}
