using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.ProjectsModels;
using MarcoPortefolioServer.Repository.v1.ProjectRepository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarcoPortefolioServer.Controllers.v1.projects
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
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
