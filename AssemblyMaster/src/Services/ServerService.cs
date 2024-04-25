using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace AssemblyMaster.Services
{    
    public class ServerService
    {
        public ServerService(){}

        public JObject GetAllServer(ServerType type)
        {   
            Server server = new Server(type);
            return server.Hosts;
        }

        public JObject GetSingle(string name)
        {   
            Server server = new Server(name);
            return server.Hosts;
        }
    }
}

