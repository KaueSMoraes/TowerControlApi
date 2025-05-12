using AssemblyMaster.Entities.DTOs;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;
using System.Collections.Generic;

namespace AssemblyMaster.Services
{
    public interface IServerService
    {
        IEnumerable<ServerDto> GetAllServers(ServerType type);
        ServerDto GetSingle(string nameServer);
    }
}
