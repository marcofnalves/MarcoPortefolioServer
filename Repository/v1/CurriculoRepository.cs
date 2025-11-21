using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.CurriculoModels;

namespace MarcoPortefolioServer.Repository.v1
{
    public class CurriculoRepository : InfoRepository
    {
        public CurriculoMainModel GetCurriculo()
        {
            List<EmpresaCurriculoModel> empresas = GetInfoEmpresas();
            CurriculoMainModel curriculo = new CurriculoMainModel();
            InfoRepository InfoRep = new InfoRepository();
            InfoModel myInfo = InfoRep.GetInfo();
            curriculo.Name = myInfo.name;
            curriculo.resumo = myInfo.resumo;
            curriculo.WorkExperiences = empresas;
            return curriculo;
        }
        public List<EmpresaCurriculoModel> GetInfoEmpresas()
        {
            List<EmpresaCurriculoModel> empresas = new List<EmpresaCurriculoModel>();
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM info";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    empresas.Add(new EmpresaCurriculoModel
                    {
                        id_empresa = reader.GetInt32(0),
                        nome_empresa = reader.GetString(1),
                        nome_cargo = reader.GetString(2),
                        descricao = reader.GetString(3),
                        data_entrada = DateTime.Parse(reader.GetString(4)),
                        data_saida = reader.IsDBNull(5) ? (DateTime?)null : DateTime.Parse(reader.GetString(5))
                    });
                }
            }
            return empresas;
        }
    }
}
