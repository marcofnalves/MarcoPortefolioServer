using MarcoPortefolioServer;
using MarcoPortefolioServer.Functions.v1;
using Microsoft.AspNetCore.Mvc;
using static MarcoPortefolioServer.Functions.v1.Age;
using MarcoPortefolioServer.Models.v1;

namespace MarcoPortefolioServer.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MyInfoController : ControllerBase
    {
        private readonly ILogger<MyInfoController> _logger;
        private readonly TokenValidator _tokenValidator;
        private readonly string tokenController = "main";

        public MyInfoController(ILogger<MyInfoController> logger, TokenValidator tokenValidator)
        {
            _logger = logger;
            _tokenValidator = tokenValidator;
        }

        // GET api/v1/info
        [HttpGet]
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
            var infoRepository = new Repository.v1.InfoRepository();
            return Ok(infoRepository);
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
            var infoRepository = new Repository.v1.InfoRepository();
            var myInfo = infoRepository.UpdateInfo(request.name, request.age, request.dateOfBirth);
            return Ok(new
            {
                valid = true,
                StatusCode = 200,
                message = "Informação atualizada com sucesso!",
                info = myInfo
            });
        }
    }
}