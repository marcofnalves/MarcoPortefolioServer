using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using System.Text.Json;

namespace MarcoPortefolioServer.Repository.v1
{
    public class VersionRepository
    {
        private readonly List<VersionModel> _versions = new();
        private readonly Server _server;

        // Constructor
        public VersionRepository(Server server)
        {
            _server = server;
            LoadVersions();
        }

        // Load everything once
        private void LoadVersions()
        {
            var dataArray = _server.getSData("versions");

            foreach (var data in dataArray)
            {
                if (data.dvalue.Length == 0)
                    continue;

                string json = data.dvalue[0];

                var versionObj = JsonSerializer.Deserialize<VersionModel>(json);

                if (versionObj != null)
                    _versions.Add(versionObj);
            }
        }

        // CREATE: add new version
        public VersionModel CreateVersion(VersionModel newVersion)
        {
            string json = JsonSerializer.Serialize(newVersion);
            _server.setSData("versions", new[] { json });
            _versions.Add(newVersion);
            return newVersion;
        }

        // GET
        public VersionModel? GetLastVersion()
        {
            return _versions.OrderByDescending(v => v.id_version).FirstOrDefault();
        }

        public VersionModel? GetVersionById(int id)
        {
            return _versions.FirstOrDefault(v => v.id_version == id);
        }

        public List<VersionModel> GetAll()
        {
            return _versions;
        }
    }
}