using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ppt23.Client.ViewModels;

public class VybaveniVM
{
    [Required] 
    [MinLength(5, ErrorMessage = "Délka u pole \"{0}\" musí být alespoň {1} znaků")]
    [Display(Name = "Název")]
    public string Name { get; set; } = "";
    public DateTime BoughtDateTime { get; set; }
    public DateTime LastRevisionDateTime { get; set; }

    public bool IsInEditMode { get; set; } = false;

    public bool IsInNewMode { get; set; } = false;

    [Required, Range(0,10000000, ErrorMessage= "Cena musí být v rozmezí 0-10000000")]
    public int Cena { get; set; }

    public bool IsRevisionNeeded 
    { 
        get 
        {
            int years = 2;
            long milisecondsToYears = 31556926000;
            if ((DateTime.Now.Subtract((DateTime)LastRevisionDateTime)).TotalMilliseconds >= years * milisecondsToYears)
            {
                return true;
            }
            return false;
        } 
    }

    public VybaveniVM(string Name, DateTime BoughtDateTime, DateTime LastRevisionDateTime, bool isInEditMode = false, int Cena = 0)
    {
        this.Name = Name;
        this.BoughtDateTime = BoughtDateTime;
        this.LastRevisionDateTime = LastRevisionDateTime;
        this.IsInEditMode = isInEditMode;
        this.Cena = Cena;
    }

    public VybaveniVM(bool isInEditMode = false)
    {
        this.Name = "Nove vybaveni";
        this.BoughtDateTime = DateTime.Now;
        this.LastRevisionDateTime = DateTime.Now;
        this.IsInEditMode = isInEditMode;
        this.Cena = 0;
    }

    public static List<VybaveniVM> VratRandSeznam(int pocet)
    {
        Random rnd = new Random();
        List<VybaveniVM> listVybaveni = new List<VybaveniVM>();
        for (int i = 0; i < pocet; ++i)
        {
            VybaveniVM vybaveni = new VybaveniVM(VytvorRandomJmeno(),VytvorRandomDatumNakupu(), VytvorRandomDatumPosledniRevize());
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
        VybaveniVM to = new(this.Name, this.BoughtDateTime, this.LastRevisionDateTime, this.IsInEditMode, this.Cena);
        return to;
    }

    public void MapTo(VybaveniVM? to)
    {
        if (to == null) return;
        to.BoughtDateTime = BoughtDateTime;
        to.LastRevisionDateTime = LastRevisionDateTime;
        to.Name = Name;
        to.Cena = Cena; 
        to.IsInEditMode = IsInEditMode;
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
