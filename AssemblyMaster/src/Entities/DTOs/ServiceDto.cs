namespace AssemblyMaster.Entities.DTOs
{
    public class ServiceDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string IP { get; set; }
        public IEnumerable<int> Ports { get; set; }
    }
}
