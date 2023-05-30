using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ppt23.Shared;

public class VybaveniVM
{
    public Guid Id { get; set; }
    [Required] 
    [MinLength(5, ErrorMessage = "Délka u pole \"{0}\" musí být alespoň {1} znaků")]
    public string Name { get; set; } = "";
    public DateTime BoughtDateTime { get; set; }
    public DateTime? LastRevisionDateTime { get; set; }

    [Required, Range(0,10000000, ErrorMessage = "Cena musí být v rozmezí 0-10000000")]
    public int Cena { get; set; }

    public bool IsRevisionNeeded 
    { 
        get 
        {
            if (LastRevisionDateTime == null)
            {
                return true;
            }
            int years = 2;
            long milisecondsToYears = 31556926000;
            if ((DateTime.Now.Subtract((DateTime)LastRevisionDateTime)).TotalMilliseconds >= years * milisecondsToYears)
            {
                return true;
            }
            return false;
        } 
    }

    public VybaveniVM(Guid id, string Name, DateTime BoughtDateTime, DateTime? LastRevisionDateTime, int Cena = 0)
    {
        Id = id;
        this.Name = Name;
        this.BoughtDateTime = BoughtDateTime;
        this.LastRevisionDateTime = LastRevisionDateTime;
        this.Cena = Cena;
    }

    public VybaveniVM()
    {
        Id = Guid.NewGuid();
        this.Name = "";
        this.BoughtDateTime = DateTime.Now;
        this.LastRevisionDateTime = null;
        this.Cena = 0;
    }

    public static List<VybaveniVM> VratRandSeznam(int pocet, bool emptyId = false)
    {
        Random rnd = new Random();
        List<VybaveniVM> listVybaveni = new List<VybaveniVM>();
        Guid guid;
        for (int i = 0; i < pocet; ++i)
        {
            guid = emptyId == false ? Guid.NewGuid() : Guid.Empty; 
            VybaveniVM vybaveni = new VybaveniVM(guid, VytvorRandomJmeno(),VytvorRandomDatumNakupu(), null);
            listVybaveni.Add(vybaveni);
        }

        return listVybaveni;
    }

    public static string VytvorRandomJmeno()
    {
        int delkaJmena = Random.Shared.Next(10,21);
        string name = "";
        for (int i = 0; i < delkaJmena; ++i)
        {
            name += (char)Random.Shared.Next(97, 123);
        }
        return name;
    }
    public static DateTime VytvorRandomDatumNakupu()
    {
        int rok = Random.Shared.Next(2000, 2019);
        int mesic = Random.Shared.Next(1, 13);
        int den = Random.Shared.Next(1, 29);

        return new DateTime(rok, mesic, den);
    }
    public static DateTime VytvorRandomDatumPosledniRevize()
    {
        int rok = Random.Shared.Next(2019, 2023);
        int mesic = Random.Shared.Next(1, 13);
        int den = Random.Shared.Next(1, 29);

        return new DateTime(rok, mesic, den);
    }
    public VybaveniVM Copy()
    {
        VybaveniVM to = new(this.Id, this.Name, this.BoughtDateTime, this.LastRevisionDateTime, this.Cena);
        return to;
    }

    public void MapTo(VybaveniVM? to)
    {
        if (to == null) return;
        to.BoughtDateTime = BoughtDateTime;
        to.LastRevisionDateTime = LastRevisionDateTime;
        to.Name = Name;
        to.Cena = Cena; 
    }

    public bool IsValid()
    {
        if (this.Name.Length < 5 || this.Cena < 0 || this.Cena > 100000000)
        {
            return false;
        }
        return true;
    }
}

public class VybaveniSrevizemaVM
{
    public string Name { get; set; } = "";
    public Guid Id { get; set; }
    //public bool IsRevisionNeeded { get => DateTime.Now.AddYears(-2) < LastRevisionDateTime; }
    public DateTime BoughtDateTime { get; set; }
    public List<RevizeViewModel> Revizes { get; set; } = new();
    public int Cena { get; set; }
    //public List<UkonVm> Ukons { get; set; } = new();
}
