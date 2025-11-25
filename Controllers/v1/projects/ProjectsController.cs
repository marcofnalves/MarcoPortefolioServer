using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Functions.v1.lib.client;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.ProjectsModels;
using MarcoPortefolioServer.Repository.v1.ProjectRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarcoPortefolioServer.Controllers.v1.projects
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "client";
        private readonly Server _server;
        private readonly Client _client;

        public ProjectsController(ILogger<ProjectsController> logger, TokenValidator tokenValidator, Server server, Client client)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
            _server = server;
            _client = client;
        }

        [HttpGet("getAllIde")]
        public List<IDEModel> GetAllIde([FromHeader] string token)
        {
            return IdeRepository.getAllIde();
        }

        [HttpPost("createNewIde")]
        public IDEModel CreateNewModel([FromHeader] string token, [FromBody] IDEModel newDataIde)
        {

            IDEModel newIde = new IDEModel();
            newIde.name = newDataIde.name;
            return newIde;
        }
    }
}
