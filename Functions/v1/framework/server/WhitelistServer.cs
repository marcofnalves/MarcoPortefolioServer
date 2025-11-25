using System;

namespace MarcoPortefolioServer.Functions.v1.framework.server
{
    public class WhitelistServer
    {
        static WhitelistServer()
        {
            RegisterServerEventInternal("setWhitelist", (string license) =>
            {
                Console.WriteLine("Recebi o license: " + license);
                TriggerClientEventInternal("setWhitelist", license);
            });
        }
    }
}