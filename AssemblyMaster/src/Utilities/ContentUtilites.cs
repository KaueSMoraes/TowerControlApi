using System.Text.RegularExpressions;
using AssemblyMaster.Entities;
using AssemblyMaster.Enums;

namespace AssemblyMaster.Utilities
{
    public static class ContentUtilites
    {
        public static string ExtractEnvironment(string nameServer)
        {
            // Regex para identificar os padrões -qa, -pre, -int, -rc
            var regex = new Regex(@"-(..|..|..|..)\b", RegexOptions.IgnoreCase);

            // Busca a correspondência
            var match = regex.Match(nameServer);
            if (match.Success)
            {
                // Retorna a parte correspondente em letras maiúsculas
                return match.Groups[1].Value.ToLower();
            }else
            {
                regex = new Regex(@"(|..|..|..|...|...|..)\b", RegexOptions.IgnoreCase);
                var matchProd = regex.Match(nameServer);
                if(matchProd.Success)
                {
                    return "prod";
                }
            }
            return null;
        }

        public static List<TypeService> ExtractServicesHost(string nameserver)
        {   
            var services = new List<TypeService>();
    
            if (nameserver.Substring(0,12).Equals("..")) 
            {
                services.AddRange(new[]
                {
                    TypeService.appbrokerdesktop,
                    TypeService.appcompilacao,
                    TypeService.dbaccess_primary,
                    TypeService.dbaccess_secondary
                });
            }
            else if (nameserver.Substring(0,12).Equals(".."))
                services.AddRange(new[]
                {
                    TypeService.appsecondary01,
                    TypeService.appsecondary02,
                    TypeService.dbaccess_secondary
                });
            else if (nameserver.Substring(0,11).Equals(".."))
                services.AddRange(new[]
                {
                    TypeService.apprest01,
                    TypeService.apprest02,
                    TypeService.apprest03,
                    TypeService.apprest04,
                    TypeService.dbaccess_secondary
                });
            else if(nameserver.Substring(0,12).Equals(".."))
                services.AddRange(new[]
                {
                    TypeService.appgravabatch,
                    TypeService.appschedule,
                    TypeService.dbaccess_secondary
                });
            else if(nameserver.Substring(0,7).Equals(".."))
                services.AddRange(new[]
                {
                    TypeService.appbrokertss,
                    TypeService.appsrvtss01,
                    TypeService.dbaccess_secondary,
                    TypeService.dbaccess_primary,
                    TypeService.appsrvtss02
                });
            else if(nameserver.Substring(0,7).Equals(".."))
                 services.AddRange(new[]
                {
                    TypeService.appbrokertss,
                    TypeService.appsrvtss01,
                    TypeService.dbaccess_secondary,
                    TypeService.dbaccess_primary,
                    TypeService.appsrvtss02
                });
            return services;
        }

        public static List<TypeService> TransformService(string service)
        {
            var services = new List<TypeService>();
            // Tenta converter a string para o tipo enumerado TypeService
            TypeService serviceEnum = (TypeService)Enum.Parse(typeof(TypeService), service, true); // 'true' para ignorar case
            services.Add(serviceEnum);
        
            return services;
        }
    }
}