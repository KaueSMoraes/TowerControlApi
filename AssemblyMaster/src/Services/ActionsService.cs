using System.Diagnostics;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using AssemblyMaster.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using AssemblyMaster.Entities.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyMaster.Services
{
    public class ActionsService : IActionsService
    {
       public ServerType Environment { get; set; }
       public ProcessTerminal Connection { get; set; }
       public TypeAction Action { get; set; }
       public List<TypeService> ServicesHost { get; set; }
       
       public ActionsService(){}

        //Utilizo construtores customizados pois o controller precisa construir esse objeto como null 
        public void BuilderActionsService(string nameserver)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.ExtractServicesHost(nameserver);
        }

        public void BuilderActionsService(string nameserver, string serviceName)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.TransformService(serviceName);
        }

        public void BuilderActionsService(string nameserver, string serviceName, TypeAction action)
        {
            Environment = ServerType.FromString(ContentUtilites.ExtractEnvironment(nameserver));
            Connection = new ProcessTerminal(nameserver);
            ServicesHost = ContentUtilites.TransformService(serviceName);
            Action = action;
        }

       public IEnumerable<ServiceDto> GetAllServices()
       {
            JObject oJson = Connection.GetAllExecute(ServicesHost, Environment == ServerType.Production);
            if (oJson == null) return Enumerable.Empty<ServiceDto>();
            // Mapear JObject para lista de ServiceDto
            return oJson.Properties().Select(p => new ServiceDto {
                Name = p.Name,
                Status = p.Value["Status"]?.ToString() ?? string.Empty,
                IP = p.Value["IP"]?.ToString() ?? string.Empty,
                Ports = p.Value["Ports"]?.ToObject<List<int>>() ?? new List<int>()
            });
       }

        public ServiceDto GetService(string serviceName)
        {
            var all = GetAllServices();
            return all.FirstOrDefault(s => s.Name == serviceName) ?? new ServiceDto();
        }

        public object ExecuteAction()
        {
            return new { success = true, action = Action.ToString(), message = $"Ação '{Action}' executada com sucesso." };
        }
    }
}