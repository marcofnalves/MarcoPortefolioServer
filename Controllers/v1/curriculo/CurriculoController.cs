using Auth0.ManagementApi.Models;
using MarcoPortefolioServer.Controllers.v1.tokens;
using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.client;
using MarcoPortefolioServer.Models.fivem;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.CurriculoModels;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.Json;
using System.Text.Json.Nodes;
using Client = MarcoPortefolioServer.Functions.v1.modules.client.Client;

namespace MarcoPortefolioServer.Controllers.v1.curriculo
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CurriculoController : ControllerBase
    {
        private readonly Client _client;
        private readonly TokenValidator _tokenValidator;
        private string tokenController = "client";
        public CurriculoController(TokenValidator tokenValidator, Client client)
        {
            _client = client;
            _tokenValidator = tokenValidator;
        }

        [HttpGet("getUData")]
        public ActionResult<DataModel> getUData([FromHeader] string token, [FromHeader] string dkey)
        {
            var data = _client.getUData(dkey);

            if (!_tokenValidator.IsValid(tokenController, token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    StatusCode = 401,
                    message = "Token inválido!"
                });
            }
            return Ok(data);
        }

        [HttpGet("getDataClientObject")]
        public ActionResult<List<CurriculoMainModel>> GetDataClientObject(
            [FromHeader] string token,
            [FromHeader] string key)
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
            DataModel[] dataArray = _client.getUData(key);
            if (dataArray.Length == 0)
                return Ok(new List<CurriculoMainModel>());

            List<CurriculoMainModel> list = new();
            foreach (var data in dataArray)
            {
                if (data.dvalue.Length == 0)
                    continue;

                string json = data.dvalue[0];
                var curriculo = JsonSerializer.Deserialize<CurriculoMainModel>(json);

                if (curriculo != null)
                    list.Add(curriculo);
            }

            return Ok(list);
        }

        [HttpPost("insertDataClientObject")]
        public ActionResult<DataModel?> InsertDataClientObject([FromHeader] string token, [FromBody] CurriculoMainModel curriculo)
        {
            string tokenTmpController = "server";
            if (!_tokenValidator.IsValid(tokenTmpController, token))
            {
                return Unauthorized(new
                {
                    valid = false,
                    StatusCode = 401,
                    message = "Token inválido!"
                });
            }

            string json = JsonSerializer.Serialize(curriculo);
            var result = _client.setUData("curriculo", new[] { json });
            return Ok(result);
        }
    }
}
