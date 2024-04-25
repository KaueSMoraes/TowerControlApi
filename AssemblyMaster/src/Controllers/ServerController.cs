using Microsoft.AspNetCore.Mvc;
using AssemblyMaster.Services;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Extensions;
using Newtonsoft.Json.Linq;

namespace AssemblyMaster.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ServerController : ControllerBase
    {
        private readonly ServerService _serverService;

        public ServerController(ServerService serverservice)
        {
            _serverService = serverservice;
        }

        // GET: /server?filter=value
        [HttpGet("server")]
        public IActionResult GetAllServer([FromQuery] TypeEnvironment filter)
        {
            try
            {   if (!filter.ToString().Equals("noENV"))
                {
                    ServerType serverType = ServerType.FromString(filter.Equals(TypeEnvironment.dev) ? ".." : filter.ToString());
                    var result = _serverService.GetAllServer(serverType); //Retorna todos os Hosts de uma base específica

                    if(!ModelState.IsValid)
                        return BadRequest(ModelState);
                    else
                        return Ok(result);
                }
                else
                {
                    return StatusCode(404, new JObject{["message"] = "Ambiente não encontrado"});
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
                throw; 
            }
        }

        // GET: /server
        [HttpGet("server/{nameServer}")]
        public IActionResult GetAll([FromRoute] string nameServer)
        {
            try
            {
                var result = _serverService.GetSingle(nameServer);
                
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                else if ((string)result[nameServer] == "Not Found")
                    return BadRequest(result);
                else
                    return Ok(result); 
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
                throw; 
            }
        }
    }
}