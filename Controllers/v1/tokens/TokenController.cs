using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MarcoPortefolioServer.Controllers.v1.tokens
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "server";
        private readonly Server _server;
        public TokenController(ILogger<TokenController> logger, TokenValidator tokenValidator, Server server)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
            _server = server;
        }

        [HttpGet("getSData")]
        public ActionResult<DataModel> getSData([FromHeader] string key, [FromHeader] string token)
        {
            var data = _server.getSData(key);
            return Ok(data);
        }

        // POST api/v1/token/createNewToken
        [HttpPost("createNewToken")]
        public ActionResult<TokenModel> CreateToken([FromHeader] string token, [FromBody] TokenModel data)
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
            if (data == null ||
                string.IsNullOrWhiteSpace(data.name) ||
                string.IsNullOrWhiteSpace(data.type) ||
                string.IsNullOrWhiteSpace(data.token) ||
                string.IsNullOrWhiteSpace(data.conta))
            {
                return BadRequest(new { message = "Campos inválidos ou incompletos." });
            }

            var repository = new TokenRepository();

            var created = repository.CreateToken(
                data.name,
                data.type,
                $"{data.type}_{TokenValidator.GenerateRandomString(20)}",
                data.conta
            );

            if (created == null)
                return Conflict(new { message = "Já existe um token com este valor para esta conta e type." });

            return Ok(created);
        }

        //GET api/v1/token/listAllTokens
        [HttpGet("listAllTokens")]
        public ActionResult<TokenModel> listAllTokens([FromHeader] string token, [FromHeader] string conta)
        {
            var repository = new TokenRepository();
            var result = repository.GetTokensByConta(conta);
            if (result == null)
                return NotFound(new { message = "Token inválido ou não encontrado." });
            return Ok(result);
        }

        // GET api/v1/token/isValidToken
        [HttpGet("isValidToken")]
        public ActionResult<TokenModel> IsValidToken(
            [FromHeader] string token, [FromHeader] TokenModel tokenModel
        )
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

            var repository = new TokenRepository();
            var result = repository.GetToken(tokenModel.token, tokenModel.type, tokenModel.conta);

            if (result == null)
                return NotFound(new { message = "Token inválido ou não encontrado." });

            return Ok(result);
        }
    }
}
