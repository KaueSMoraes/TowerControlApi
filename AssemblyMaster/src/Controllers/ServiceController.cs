using AssemblyMaster.Enums;
using AssemblyMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AssemblyMaster.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ServiceController : ControllerBase
    {
        private readonly ActionsService _actionsService;
        
        public JObject Response { get; set; }
        public ServiceController(ActionsService actionsService)
        {
            _actionsService = actionsService;
        }

        [HttpGet("server/{nameServer}/services")]
        public IActionResult GetAllServices([FromRoute] string nameServer)
        {
            try
            {
                 if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                else
                    _actionsService.BuilderActionsService(nameServer);
                    Response = _actionsService.GetAllServices();
                    return Ok(Response);
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message); 
            }
        }

        [HttpGet("server/{nameServer}/services/{serviceName}")]
        public IActionResult GetService([FromRoute] string nameServer, [FromRoute] string serviceName)
        {
            try
            {
                _actionsService.BuilderActionsService(nameServer, serviceName);
                var response = _actionsService.GetAllServices();
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                else
                    return Ok(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
            }
        }

        [HttpPost("server/{nameServer}/services/{serviceName}/{actionService}")]
        public IActionResult ActionService([FromRoute] string nameServer, [FromRoute] string serviceName, [FromRoute] TypeAction actionService)
        {
            try
            {
                _actionsService.BuilderActionsService(nameServer, serviceName, actionService);
                var response = _actionsService.ExecuteAction();
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                else
                    return Ok(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
            }
        }
    }
}