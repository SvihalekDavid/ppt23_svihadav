using Ppt23.Shared;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PPT23.API.Data
{
    public class Vybaveni
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Délka u pole \"{0}\" musí být alespoň {1} znaků")]
        public string Name { get; set; } = "";
        public DateTime BoughtDateTime { get; set; }

        public List<Revize> Revizes { get; set; } = new();

        public List<Ukon> Ukons { get; set; } = new();

        [Required, Range(0, 10000000, ErrorMessage = "Cena musí být v rozmezí 0-10000000")]
        public int Cena { get; set; }

        public static Vybaveni MakeVybaveniFromVybaveniVM(VybaveniVM vyb)
        {
            Vybaveni v = new Vybaveni();
            v.Id = vyb.Id;
            v.Name = vyb.Name;
            v.BoughtDateTime = vyb.BoughtDateTime;
            v.Cena = vyb.Cena;
            return v;
        }

        public static VybaveniVM MakeVybaveniVMFromVybaveni(Vybaveni vyb)
        {
            VybaveniVM v = new VybaveniVM();
            v.Id = vyb.Id;
            v.Name = vyb.Name;
            v.BoughtDateTime = vyb.BoughtDateTime;
            v.Cena = vyb.Cena;
            return v;
        }
    }
}
