using Discord;
using Discord.WebSocket;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord.Commands
{
    public class SayCommand : ICommand
    {
        public string Name => "say";

        public SlashCommandBuilder Build()
        {
            return new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription("O bot repete a tua mensagem.")
                .AddOption("mensagem", ApplicationCommandOptionType.String, "Mensagem a repetir", isRequired: true);
        }

        public async Task Execute(SocketSlashCommand cmd)
        {
            var msg = cmd.Data.Options.First().Value.ToString();
            await cmd.RespondAsync($"📣 {msg}");
        }
    }
}