namespace MarcoPortefolioServer.Models.v1.CurriculoModels
{
    public class EmpresaCurriculoModel
    {
        public int id_empresa { get; set; }
        public string nome_empresa { get; set; } = string.Empty;
        public string nome_cargo { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public DateTime data_entrada { get; set; }
        public DateTime? data_saida { get; set; }

    }
}
