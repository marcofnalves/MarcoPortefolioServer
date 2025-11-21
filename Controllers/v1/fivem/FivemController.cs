using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.server;
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
        [HttpGet("coming")]
        public ActionResult<string> comingsoon()
        {
            string response = "Coming Soon...";
            return Ok(response);
        }
    }
}
