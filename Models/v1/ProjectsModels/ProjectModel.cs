namespace MarcoPortefolioServer.Models.v1.ProjectsModels
{
    public class ProjectModel
    {
        public int id_project { get; set; } = 0;
        public int id_ide { get; set; } = 0;
        public int id_lang { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string icon { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
    }
}
