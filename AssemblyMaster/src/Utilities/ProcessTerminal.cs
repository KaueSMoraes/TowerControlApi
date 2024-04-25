using Renci.SshNet;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using AssemblyMaster.Enums;
using System.Web.WebPages;
using Renci.SshNet.Common;
using AssemblyMaster.Entities;

namespace AssemblyMaster.Utilities
{
    public class ProcessTerminal
    {
        public string Command { get; set; }
        public ConnectionSsh Connection { get; set; }

        public ProcessTerminal(string dns)
        {
            Connection = new ConnectionSsh(dns);
        }

        public JObject GetAllExecute(List<TypeService> services, bool lProd)
        {
            using (var client = new SshClient(Connection.connectionInfo))
            {
                JObject oServices = new JObject();
                client.Connect();
                if (client.IsConnected)
                {
                    foreach (var service in services)
                    {
                        var oService = new Service(service);
                        oService.GetStatus(client);

                        //Tratamento somente para Balances.
                        if (lProd && (service.Equals(TypeService.appsecondary01) || service.Equals(TypeService.appsecondary02)))
                            oService.GetActivity();

                        oServices[service.ToString()] = oService.ToJObject();
                    }
                }else 
                {
                    throw new InvalidOperationException("Não foi possível estabelecer conexão SSH.");
                }    

                client.Disconnect();
                client.Dispose();
                
                return oServices;
            }
        }
        
        public JObject ShellExecute(TypeAction action, TypeService service)
        {
            using (var client = new SshClient(Connection.connectionInfo))
            {
                var Result = new JObject{["result"]= false};
                client.Connect();
                if (client.IsConnected)
                {
                    var command = client.RunCommand("sudo service " + service.ToString() + " " + action.ToString());
                    Result["result"] = true;
                }
                client.Disconnect();
                client.Dispose();
                return Result;
            }
        }
    }
}