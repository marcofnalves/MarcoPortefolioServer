using System;
using MarcoPortefolioServer.Repository.v1;
namespace MarcoPortefolioServer.Models.v1.CurriculoModels
{
    public class CurriculoMainModel
    {
        public string Name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telemovel { get; set; } = string.Empty;
        public string resumo { get; set; } = string.Empty;
        public List<EmpresaCurriculoModel> WorkExperiences { get; set; } = new List<EmpresaCurriculoModel>();
    }
}
