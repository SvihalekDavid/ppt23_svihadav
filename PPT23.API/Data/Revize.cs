using Ppt23.Shared;

namespace PPT23.API.Data
{
    public class Revize
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";
        public DateTime DateTime { get; set; }

        public Guid VybaveniId { get; set; }
        public Vybaveni Vybaveni { get; set; } = null!;

        public static RevizeViewModel MakeRevizeVMFromRevize(Revize vyb)
        {
            RevizeViewModel r = new RevizeViewModel();
            r.Id = vyb.Id;
            r.Name = vyb.Name;
            return r;
        }
    }
}
