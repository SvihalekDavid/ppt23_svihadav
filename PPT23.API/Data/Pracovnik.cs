namespace PPT23.API.Data
{
    public class Pracovnik
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string Povolani { get; set; } = "";

        public List<Ukon> Ukons { get; set; } = new();
    }
}
