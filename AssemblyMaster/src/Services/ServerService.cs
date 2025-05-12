using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using AssemblyMaster.Entities.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyMaster.Services
{    
    public class ServerService : IServerService
    {
        public ServerService(){}

        public IEnumerable<ServerDto> GetAllServers(ServerType type)
        {   
            Server server = new Server(type);
            // Mapear para DTOs
            return server.Hosts.Properties().Select(h => new ServerDto {
                Name = h.Name,
                Environment = type.ToString()
            });
        }

        public ServerDto GetSingle(string name)
        {   
            Server server = new Server(name);
            var host = server.Hosts.Properties().FirstOrDefault();
            if (host == null) return new ServerDto();
            return new ServerDto {
                Name = host.Name,
                Environment = server.GetType().Name 
            };
        }
    }
}

