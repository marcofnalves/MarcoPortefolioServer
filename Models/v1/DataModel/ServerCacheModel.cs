namespace MarcoPortefolioServer.Models.v1.DataModel
{
    public class ServerCacheModel : List<DataModel>
    {
        public void Add(int id_data, string dkey, string[] dvalue)
        {
            base.Add(new DataModel
            {
                id_data = id_data,
                dkey = dkey,
                dvalue = dvalue
            });
        }

        public void Add(DataModel dm)
        {
            base.Add(dm);
        }
    }
}
