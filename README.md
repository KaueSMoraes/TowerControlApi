### TowerControlApi
Aplicação WEB API em  ASP.NET CORE para gerenciar serviços Linux.

### Gerenciamento de Servidores Linux para Aplicação Protheus

### Descrição
O projeto consiste em uma API Web em ASP .NET desenvolvida para realizar operações de gerenciamento em servidores Linux que suportam a aplicação Protheus. A API recebe requisições através de diversos controladores, os quais são direcionados para serviços específicos. Estes serviços atuam como ponte com as entidades do sistema, utilizando construtores avançados para manipular objetos e retornar status e resultados para os controladores.

### Funcionalidades Principais
- Gerenciamento de servidores Linux.
- Integração com a aplicação Protheus.
- Utilização de serviços para manipulação de entidades.
- Retorno de status e resultados para os controladores.

### Tecnologias Utilizadas
- ASP .NET
- Serviços RESTful
- Servidores Linux
- Aplicação Protheus

### Frameworks e Bibliotecas Adicionais
- SSH.NET
- Swashbuckle.AspNetCore
- Microsoft.AspNetCore.OpenApi
- Newtonsoft.Json

## ApiController
Neste exemplo abaixo mostra como os controllers devem ser contruídos: 
- Definição da Rota Principal da API
- Content-Type de gerenciamento (application/json);
- Objeto ou classe de Service deve ser instanciado como o "Service" principal do controller.
- Encapsulamento do objeto de Serviço
- Try-Cath para tratamento de Exceptions que podem ser geradas na pilha de chamadas do controller.
- Verbos HTTPS que refletem o retorno correto da Api
  
````csharp
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
                    ServerType serverType = ServerType.FromString(filter.Equals(TypeEnvironment.dev) ? "int" : filter.ToString());
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
```

## Services

## Entities

## Authentication





