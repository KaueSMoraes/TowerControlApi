using System.Text;
using AssemblyMaster.Enums;
using AssemblyMaster.Utilities;
using Newtonsoft.Json.Linq;
using AssemblyMaster.Entities;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

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