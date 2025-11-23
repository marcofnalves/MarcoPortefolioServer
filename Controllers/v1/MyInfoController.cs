using Auth0.ManagementApi.Models.Keys;
using MarcoPortefolioServer;
using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.client;
using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.CurriculoModels;
using MarcoPortefolioServer.Models.v1.DataModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static MarcoPortefolioServer.Functions.v1.Age;

namespace MarcoPortefolioServer.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MyInfoController : ControllerBase
    {
        private readonly ILogger<MyInfoController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "client";
        private readonly Server _server;
        private readonly Client _client;

        public MyInfoController(ILogger<MyInfoController> logger, TokenValidator tokenValidator, Server server, Client client)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
            _server = server;
            _client = client;
        }

        // GET api/v1/info
        [HttpGet("getInfo")]
        public ActionResult<InfoModel> GetInfo([FromHeader] string token)
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

            DataModel[] dataArray = _client.getUData("myinfo");
            if (dataArray.Length == 0)
                return Ok(new List<InfoModel>());

            List<InfoModel> list = new();
            foreach (var data in dataArray)
            {
                if (data.dvalue.Length == 0)
                    continue;

                string json = data.dvalue[0];
                var myInfo = JsonSerializer.Deserialize<InfoModel>(json);

                if (myInfo != null)
                    list.Add(myInfo);
            }

            return Ok(list);
        }

        //POST api/v1/main/updateInfo
        [HttpPost("updateInfo")]
        public ActionResult UpdateInfo(
            [FromHeader] string token,
            [FromBody] InfoModel request)
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
            string json = JsonSerializer.Serialize(request);
            var result = _client.setUData("myinfo", new[] { json });
            return Ok(new
            {
                valid = true,
                StatusCode = 200,
                message = "Informação atualizada com sucesso!",
                info = request
            });
        }
    }
}