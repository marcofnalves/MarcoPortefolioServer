using MarcoPortefolioServer.Models.fivem;

namespace MarcoPortefolioServer.Functions.v1.framework.client
{
    public class WhitelistClient
    {
        public WhitelistClient()
        {
            RegisterClientEventInternal("setWhitelist", (object[] args) =>
            {
                if (args[0] is WhitelistModel whitelist)
                {
                    Console.WriteLine($"Whitelist recebida para o license: {whitelist.license}");
                }
                else
                {
                    Console.WriteLine("Erro: objeto não é um WhitelistModel.");
                }
            });
        }
    }
}