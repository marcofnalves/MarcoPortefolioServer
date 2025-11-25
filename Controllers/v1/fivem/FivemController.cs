using MarcoPortefolioServer.Functions.v1.framework.server;
using MarcoPortefolioServer.Functions.v1.framework.client;
using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Functions.v1.lib.client;
using MarcoPortefolioServer.Models.fivem;
using Microsoft.AspNetCore.Mvc;

namespace MarcoPortefolioServer.Controllers.v1.fivem
{
    [ApiController]
    [Route("api/v1/fivem/[controller]")]
    public class FivemController : ControllerBase
    {
        private readonly ILogger<FivemController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "server";
        private readonly Server _server;

        public FivemController(ILogger<FivemController> logger, TokenValidator tokenValidator, Server server)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
            _server = server;
        }

        [HttpPost("newWhitelist")]
        public async Task<ActionResult<object>> newWhitelist([FromHeader] string token, WhitelistModel whitelist)
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

            string? license = whitelist.license;
            string? discord = whitelist.discord;
            string? steam = whitelist.steam;
            string? license2 = whitelist.license2;

            object result = null;

            await Task.Run(() =>
            {
                TriggerClientCallbackInternal("client:setWhitelist", new object[] { license, discord, steam, license2 }, (callbackResult) =>
                {
                    Console.WriteLine(callbackResult.ToString());
                    result = callbackResult;
                });
            });

            if (result != null)
            {
                return Ok(new { message = "Whitelist adicionada com sucesso", data = result });
            }
            else
            {
                return BadRequest(new { message = "Falha ao adicionar whitelist" });
            }
        }
    }
}