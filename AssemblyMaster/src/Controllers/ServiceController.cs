using AssemblyMaster.Enums;
using AssemblyMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyMaster.Controllers
{
    /// <summary>
    /// Controller responsável por operações de serviços em servidores.
    /// Permite listar serviços, obter detalhes e executar ações em serviços.
    /// </summary>
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class ServiceController : ControllerBase
    {
        private readonly IActionsService _actionsService;

        public ServiceController(IActionsService actionsService)
        {
            _actionsService = actionsService;
        }

        /// <summary>
        /// Lista todos os serviços de um servidor.
        /// </summary>
        /// <param name="nameServer">Nome do servidor.</param>
        /// <returns>Lista de serviços.</returns>
        [HttpGet("server/{nameServer}/services")]
        public IActionResult GetAllServices([FromRoute] string nameServer)
        {
            _actionsService.BuilderActionsService(nameServer);
            var result = _actionsService.GetAllServices();
            return Ok(result);
        }

        /// <summary>
        /// Retorna detalhes de um serviço específico.
        /// </summary>
        /// <param name="nameServer">Nome do servidor.</param>
        /// <param name="serviceName">Nome do serviço.</param>
        /// <returns>Detalhes do serviço.</returns>
        [HttpGet("server/{nameServer}/services/{serviceName}")]
        public IActionResult GetService([FromRoute] string nameServer, [FromRoute] string serviceName)
        {
            _actionsService.BuilderActionsService(nameServer, serviceName);
            var result = _actionsService.GetService(serviceName);
            if (string.IsNullOrEmpty(result?.Name))
                return NotFound(new { message = "Serviço não encontrado" });
            return Ok(result);
        }

        /// <summary>
        /// Executa uma ação (start, stop, restart) em um serviço do servidor.
        /// </summary>
        /// <param name="nameServer">Nome do servidor.</param>
        /// <param name="serviceName">Nome do serviço.</param>
        /// <param name="actionService">Ação a ser executada.</param>
        /// <returns>Resultado da execução da ação.</returns>
        [HttpPost("server/{nameServer}/services/{serviceName}/{actionService}")]
        public IActionResult ActionService([FromRoute] string nameServer, [FromRoute] string serviceName, [FromRoute] TypeAction actionService)
        {
            _actionsService.BuilderActionsService(nameServer, serviceName, actionService);
            var response = _actionsService.ExecuteAction();
            return Ok(response);
        }
    }
}