using Auth0.AspNetCore.Authentication;
using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1;
using Microsoft.AspNetCore.Mvc;
using MarcoPortefolioServer.Models.v1;
using System.Text.Json;

namespace MarcoPortefolioServer.Controllers.v1.version
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "server";
        private readonly Server _server;
        private readonly VersionRepository _versionRepo;

        public VersionController(ILogger<VersionController> logger, TokenValidator tokenValidator,Server server, VersionRepository versionRepo)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
            _versionRepo = versionRepo;
            _server = server;
        }

        [HttpGet("lastVersion")]
        public ActionResult<VersionModel> getLastVersion(
            [FromHeader] string token)
        {
            if (!_tokenValidator.IsValid(tokenController, token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    StatusCode = 401,
                    message = "Token inválido!"
                });
            }

            DataModel[] dataArray = _server.getSData("versions");
            List<VersionModel> list = new List<VersionModel>();
            foreach (var data in dataArray)
            {
                if (data.dvalue.Length == 0)
                    continue;

                string json = data.dvalue[0];
                var myInfo = JsonSerializer.Deserialize<VersionModel>(json);

                if (myInfo != null)
                    list.Add(myInfo);
            }               
            return Ok(list);
        }

        [HttpGet("allVersions")]
        public ActionResult<VersionModel> getAllVersions(
            [FromHeader] string token)
        {
            if (!_tokenValidator.IsValid(tokenController, token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    StatusCode = 401,
                    message = "Token inválido!"
                });
            }
            DataModel[] dataArray = _server.getSData("versions");
            List<VersionModel> list = new List<VersionModel>();
            foreach (var data in dataArray)
            {
                if (data.dvalue.Length == 0)
                    continue;
                string json = data.dvalue[0];
                var myInfo = JsonSerializer.Deserialize<VersionModel>(json);
                if (myInfo != null)
                    list.Add(myInfo);
            }
            return Ok(list);
        }

        [HttpPost("newVersion")]
        public ActionResult<VersionModel> CreateVersion(
            [FromHeader] string token,
            [FromHeader] string version,
            [FromHeader] string name,
            [FromHeader] string description
        )
        {
            if (!_tokenValidator.IsValid("server", token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    message = "Token inválido!"
                });
            }
            int lastVersion = _versionRepo.GetLastVersion()+1;
            VersionModel newVersion = new VersionModel
            {
                id_version = lastVersion,
                version = version,
                name = name,
                description = description
            };
            _versionRepo.CreateVersion(newVersion);
            return Ok(new
            {
                valid = true,
                message = "Nova versão criada com sucesso!",
                version = newVersion
            });
        }

        // POST api/v1/version/update
        [HttpPost("update")]
        public ActionResult UpdateVersion(
            [FromHeader] string token,
            [FromBody] VersionModel request)
        {
            if (!_tokenValidator.IsValid("setVersion", token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    message = "Token inválido!"
                });
            }

            //versionInfo.number = request.number;
            //versionInfo.name = request.name;
            //versionInfo.description = request.description;

            return Ok(new
            {
                valid = true,
                message = "Versão atualizada com sucesso!",
                //version = versionInfo
            });
        }
    }
}