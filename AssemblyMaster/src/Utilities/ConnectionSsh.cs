using System.IO;
using Renci.SshNet;

namespace AssemblyMaster.Utilities
{
    public class ConnectionSsh
    {
        public string Host { get; set;}
        public string Username { get; set; }
        public string PrivateKeyFilePath { get; set; }
        public PrivateKeyFile privateKeyFile { get; set; }
        
        public Renci.SshNet.ConnectionInfo connectionInfo { get; set; }

        public ConnectionSsh(string dns)
        {
            Host = dns ;
            Username = "...";
            PrivateKeyFilePath = "..";
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(PrivateKeyFilePath);

            PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(Username, privateKeyFile);
            connectionInfo = new Renci.SshNet.ConnectionInfo(Host, Username, privateKeyAuth);
        }
    }
}

