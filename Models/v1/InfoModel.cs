using System;

namespace MarcoPortefolioServer
{
    public class InfoModel
    {
        public string name { get; set; } = string.Empty;
        public string resumo { get; set; } = string.Empty;
        public int age { get; set; }
        public DateOnly dateOfBirth { get; set; }
    }
}