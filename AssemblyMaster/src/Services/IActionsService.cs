using AssemblyMaster.Entities.DTOs;
using AssemblyMaster.Enums;
using System.Collections.Generic;

namespace AssemblyMaster.Services
{
    public interface IActionsService
    {
        void BuilderActionsService(string nameServer);
        void BuilderActionsService(string nameServer, string serviceName);
        void BuilderActionsService(string nameServer, string serviceName, TypeAction action);
        IEnumerable<ServiceDto> GetAllServices();
        ServiceDto GetService(string serviceName);
        object ExecuteAction();
    }
}
