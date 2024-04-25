using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using AssemblyMaster.Enums;
using HtmlAgilityPack;
using IdentityServer4.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace AssemblyMaster.Entities
{
    public class Service
    {
        public JArray Ports { get; set; }
        public string Activity { get; set; }
        public string IP {get; set;}
        public  string Status { get; set; }
        public TypeService NameService { get; set; }

        public Service(TypeService nameservice)
        {
            NameService = nameservice;
        }


        public Service GetStatus(SshClient client) 
        {
            var command = client.RunCommand("sudo service " + NameService.ToString() + " status"); 
            Regex regexStatus = new Regex(@"Status process\s*:\s*\[\s*([^\]]+)\s*\]");
            if (string.IsNullOrEmpty(command.Result))
            {
                throw new SshException(command.Error);
            } 

            Match matchStatus = regexStatus.Match(command.Result);
            Status = matchStatus.Success ? matchStatus.Groups[1].Value : "";

            // Regex para capturar as portas
            Regex regexPorts = new Regex(@"PORT\s*:\s*([\d\s]+)");
            Match matchPorts = regexPorts.Match(command.Result);

            if (matchPorts.Success)
            {
                Ports = new JArray();
                string[] ports = matchPorts.Groups[1].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string port in ports)
                {
                    if (int.TryParse(port, out int portNumber))
                    {
                        this.Ports.Add(portNumber);
                    }
                }
            }
            IP = client.RunCommand("hostname -I").Result.ToString().Substring(0,14);
            return this;
        }

        public Service GetActivity()
        {
            var Url = "...";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(Url).Result;
                
                // Verifica se a resposta foi bem sucedida
                if (response.IsSuccessStatusCode)
                {
                    string htmlContent =  response.Content.ReadAsStringAsync().Result;

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);

                    // Assumindo que o JSON está no elemento <body>
                    var jsonText = htmlDoc.DocumentNode.SelectSingleNode("//body").InnerText;
                    
                    // Limpeza do JSON - exemplo básico, ajuste conforme necessário
                    jsonText = jsonText.Trim().Trim('"');
                    jsonText = System.Web.HttpUtility.HtmlDecode(jsonText);
                    jsonText = jsonText.Replace("\\\"", "\"");

                    Activity = this.SearchJson(this.IP + ":1000", jsonText);
                }
                else
                {
                    throw new HttpRequestException ("Erro ao obter a resposta da API: " + response.StatusCode);
                }
            }
            return this;
        }

        private string SearchJson(string ip, string jsonData)
        {
            jsonData = jsonData.Trim();
            if (jsonData.StartsWith("\"") && jsonData.EndsWith("\""))
            {
                jsonData = jsonData.Substring(1, jsonData.Length - 2);
                jsonData = jsonData.Replace("\\\"", "\""); 
            }
            
            JObject data = JObject.Parse(jsonData);
            JArray servers = (JArray)data["servers"];
            foreach (JObject server in servers)
            {
                string fullServerName = server["server"]?.ToString() + server["nameService"]?.ToString();
                if (fullServerName == ip + ((int)NameService).ToString())
                {
                    return server["quarantine"]?.ToString() == "-" ? "Active" : "Quarantine";
                }
            }
            return null;
        }
        public JObject ToJObject()
        {
            JObject jsonObject = new JObject();
            jsonObject["status"] = Status;
            if(!Activity.IsNullOrEmpty())
                jsonObject["activity"] = Activity;
            jsonObject["ports"] = Ports != null ? JToken.FromObject(Ports) : null;

            return jsonObject;
        }
    }    
}

