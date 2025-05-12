using Microsoft.AspNetCore.Mvc;
using AssemblyMaster.Services;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Extensions;
using Newtonsoft.Json.Linq;
using AssemblyMaster.Entities.DTOs;

namespace AssemblyMaster.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas a servidores.
    /// Fornece endpoints para listar servidores e obter detalhes de um servidor específico.
    /// </summary>
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;

        public ServerController(IServerService serverService)
        {
            _serverService = serverService;
        }

        /// <summary>
        /// Retorna todos os servidores de um ambiente específico.
        /// </summary>
        /// <param name="filter">Ambiente a ser filtrado.</param>
        /// <returns>Lista de servidores.</returns>
        [HttpGet("server")]
        public IActionResult GetAllServer([FromQuery] TypeEnvironment filter)
        {
            if (filter == TypeEnvironment.Other)
                return NotFound(new { message = "Ambiente não encontrado" });

            ServerType serverType = ServerType.FromString(filter.ToString());
            var result = _serverService.GetAllServers(serverType);
            return Ok(result);
        }

        /// <summary>
        /// Retorna os detalhes de um servidor específico.
        /// </summary>
        /// <param name="nameServer">Nome do servidor.</param>
        /// <returns>Detalhes do servidor.</returns>
        [HttpGet("server/{nameServer}")]
        public IActionResult GetAll([FromRoute] string nameServer)
        {
            var result = _serverService.GetSingle(nameServer);
            if (string.IsNullOrEmpty(result?.Name))
                return NotFound(new { message = "Servidor não encontrado" });
            return Ok(result);
        }
    }
}