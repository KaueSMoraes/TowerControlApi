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
- Os Verbos HTTPS devem refletir o retorno correto da Api
- Os controllers devem enviar instruções de operações para os Services.

```csharp
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
```
```

## Services
Neste projeto os Services são definidos no prórprio controller, por isso são encpasulados para serem acionados sob medida de requisição. 
As metodologias e responsabilidades do service são essas:
- Guardar a instância das Entidades, executar uma ou mais operações, e retornar para o controller o resultado.
- Exectuar eventos secundários a partir das requisições dos Controlles

```csharp

namespace AssemblyMaster.Services
{
    public class ActionsService
    {
       public ServerType Environment { get; set; }
       public ProcessTerminal Connection { get; set; }
       public TypeAction Action { get; set; }
       public List<TypeService> ServicesHost { get; set; }
       
       public ActionsService(){}

        //Utilizo construtores customizados pois o controller precisa construir esse objeto como null 
        public ActionsService BuilderActionsService(string nameserver)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.ExtractServicesHost(nameserver);
            return this;
        }

        public ActionsService BuilderActionsService(string nameserver, string serviceName)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.TransformService(serviceName);
            return this;
        }

        public ActionsService BuilderActionsService(string nameserver, string serviceName, TypeAction action)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.TransformService(serviceName);
            Action = action;
            return this;
        }

       public JObject GetAllServices()
       {
            JObject oJson = Connection.GetAllExecute(ServicesHost, Environment == ServerType.Production ? true : false);
            if (oJson != null)
            {   
                return oJson;
            }
            else
                return new JObject{["message"] = "Ocorreu um erro desconhecido"};
       }

        public JObject ExecuteAction()
        {
            JObject oJson = Connection.ShellExecute(Action, ServicesHost[0]);
            if (oJson != null)
            {   
                return oJson;
            }
            else
                return new JObject{["message"] = "Ocorreu um erro desconhecido"};
        }
    }
}

```
```
```

## Entities
As entidades ou Modelos, são os Objetos principais de uma Solution ou Assembly. Elas devidem as camadas da aplicação em objetos que refletem o "mundo-real".
Entre as características que mais definem as entidades estão:
- Definição de atributos dos Objetos, ou Propriedades.
- Construtores e suas Sobrecargas
- Herança de objetos relacionados
- Polimorfismo e tratamento divergente de objetos interrelacionados

```csharp
namespace AssemblyMaster.Entities
{
    public class Server 
    {
        private JObject _hosts; //Encapsulamento da propriedade
        public JObject Hosts
        {
            get
            {
                if (_hosts == null)
                    _hosts = new JObject();
                    
                if (Environment != null)
                {
                    _hosts["type"] = Environment.ToString();
                }
                return _hosts;
            }
            set
            {
                _hosts = value;
            }
        }

        public ServerType Environment { get; set; }

        //Construtor padrão
        public Server(ServerType environment)
        {
            Environment = environment;
            Hosts = BuildServerEnvironment();
        }

        public Server(string nameServer)
        {
            Hosts = BuildServerEnvironment(nameServer);
        }

        //Metodo padrão para buildar os servers filtrando por tipo de base
        private JObject BuildServerEnvironment()
        {
            var path = System.Environment.CurrentDirectory + "/src/" + Environment.ToString() + ".json";
            using (StreamReader sr = new StreamReader(path))
            {
                JArray jsonArray = JArray.Parse(sr.ReadToEnd());
                return (JObject)jsonArray[0];
            }
        }

        public JObject BuildServerEnvironment(string nameServer)
        {  
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameServer));
            var path =  System.Environment.CurrentDirectory + "/src/"  + Environment.ToString() + ".json" ;
            using (StreamReader sr = new StreamReader(path))
            {
                var content = sr.ReadToEnd();
                JArray jsonArray = JArray.Parse(content);
                
                // Cria um novo JObject para retornar apenas a chave-valor desejada
                JObject result = new JObject();
            
                if (jsonArray.Count > 0 && jsonArray[0] is JObject)
                {
                    JObject oJson = (JObject)jsonArray[0];

                    if (oJson[nameServer] != null)
                    {
                        result[nameServer] = oJson[nameServer].ToString();
                    }
                    else
                    {
                        result[nameServer] = "Not Found";
                    }
                }
                return result;
            }
        }
    }
}

```
```
```

## Authentication
Qualquer api de mercado, precisa que seu processo de autenticação esteja bem definido e tratado. Devido a aplicação já possuir muitos outros métodos apartados de segurança cibernética, e devido ao sensibilidade dessas informações, o modelo público do projeto visa mostrar apenas um EXEMPLO de uma autenticação Basic Auth.
- Autentica todas as requisições destinadas aos Endpoints.

### Conclusão

A TowerControlApi é uma solução robusta e avançada destinada a facilitar a gestão e manutenção de servidores Linux que suportam a aplicação Protheus, utilizando tecnologias modernas e práticas recomendadas no desenvolvimento de APIs em ASP.NET Core. Este projeto encapsula uma série de funcionalidades críticas para o gerenciamento eficiente de infraestrutura de TI, demonstrando uma arquitetura bem planejada e uma implementação cuidadosa.

Através dos controladores da API, a aplicação permite uma interação detalhada com os servidores, possibilitando operações como a consulta e manipulação de status de servidores de forma dinâmica e segura. A inclusão de tecnologias como SSH.NET e Newtonsoft.Json, juntamente com a integração do Swashbuckle.AspNetCore para geração de documentação OpenAPI, reforça a capacidade da API de ser tanto flexível quanto poderosa.

O design cuidadoso dos Services e Entities reflete a complexidade e as necessidades específicas do gerenciamento de sistemas que suportam o Protheus, enfatizando a segurança, a modularidade e a reutilização de código. Isso é particularmente evidente na maneira como os serviços são construídos e encapsulados, garantindo que os controladores possam responder de maneira eficiente e segura às requisições do cliente.

Por fim, o sistema de autenticação implementado serve como uma camada fundamental de segurança, assegurando que apenas usuários autorizados possam acessar funcionalidades críticas. Este projeto não apenas atende às necessidades atuais de administração de sistemas mas também estabelece uma base sólida para futuras expansões e integrações, demonstrando uma excelente aplicação das capacidades do ASP.NET Core em ambientes de produção reais.



