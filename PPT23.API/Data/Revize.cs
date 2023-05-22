using Ppt23.Shared;

namespace PPT23.API.Data
{
    public class Revize
    {
        public Guid Id { get; set; }

        //
        public string Name { get; set; } = "";

        public Revize(Guid id, string Name)
        {
            this.Id = id;
            this.Name = Name;
        }

        public static RevizeViewModel MakeRevizeVMFromRevize(Revize vyb)
        {
            RevizeViewModel r = new RevizeViewModel();
            r.Id = vyb.Id;
            r.Name = vyb.Name;
            return r;
        }
    }
}
