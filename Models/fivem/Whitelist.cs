namespace MarcoPortefolioServer.Models.fivem
{
    public class Whitelist
    {
        public string license { get; set; } = string.Empty;
        public string license2 { get; set; } = string.Empty;
        public string discord { get; set; } = string.Empty;
        public string steam { get; set; } = string.Empty;
        public int whitelisted { get; set; } = 1;
        public int banned { get; set; } = 0;
    }
}
