namespace AssemblyMaster.Entities
{
    public class ServerType
    {
        public static readonly ServerType Production = new ServerType("..");
        public static readonly ServerType QualityAssurance = new ServerType("..");
        public static readonly ServerType PreProd = new ServerType("..");
        public static readonly ServerType Development = new ServerType("..");
        public static readonly ServerType RC = new ServerType("..");

        private string Name { get; }

        private ServerType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        // Método para encontrar uma instância de ServerType baseado em um nome// Método estático para converter uma string em um ServerType
        public static ServerType FromString(string name)
        {
            switch (name.ToLower())
            {
                case "..":
                    return Production;
                case "..":
                    return QualityAssurance;
                case "..":
                    return PreProd;
                case "..":
                    return Development;
                case "..":
                    return RC;    
                default:
                    throw new ArgumentException("Invalid server type provided");
            }
        }
    }
}