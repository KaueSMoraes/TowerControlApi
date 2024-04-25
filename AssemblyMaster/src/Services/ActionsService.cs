using System.Diagnostics;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using AssemblyMaster.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

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