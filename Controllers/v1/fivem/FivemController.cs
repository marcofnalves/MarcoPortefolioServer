using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Models.fivem;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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

        // GET api/v1/version
        [HttpPost("newWhitelist")]
        public ActionResult<string> newWhitelist([FromHeader] string token, WhitelistModel whitelist)
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

            var response = _server.NewWhitelist(license, license2, steam, discord);
            return Ok(response);
        }
    }
}
