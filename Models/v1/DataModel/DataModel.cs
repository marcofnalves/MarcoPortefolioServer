using System.Text.Json;
using System.Text.Json.Nodes;

namespace MarcoPortefolioServer.Models.v1.DataModel
{
    public class DataModel
    {
        public int id_data { get; set; }
        public string dkey { get; set; } = string.Empty;
        public string[] dvalue { get; set; } = Array.Empty<String>();
    }
}
