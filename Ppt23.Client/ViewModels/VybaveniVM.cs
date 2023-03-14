namespace Ppt23.Client.ViewModels;

public class VybaveniVM
{
    public string Name { get; set; } = "";
    public DateTime BoughtDateTime { get; set; }
    public DateTime LastRevisionDateTime { get; set; }
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

    public VybaveniVM(string Name, DateTime BoughtDateTime, DateTime LastRevisionDateTime)
    {
        this.Name = Name;
        this.BoughtDateTime = BoughtDateTime;
        this.LastRevisionDateTime = LastRevisionDateTime;
    }

    public static List<VybaveniVM> VratRandSeznam(int pocet)
    {
        Random rnd = new Random();
        List<VybaveniVM> listVybaveni = new List<VybaveniVM>();
        for (int i = 0; i < pocet; ++i)
        {
            VybaveniVM vybaveni = new VybaveniVM(VytvorRandomJmeno(rnd),VytvorRandomDatumNakupu(rnd), VytvorRandomDatumPosledniRevize(rnd));
            listVybaveni.Add(vybaveni);
        }

        return listVybaveni;
    }

    private static string VytvorRandomJmeno(Random rnd)
    {
        int delkaJmena = rnd.Next(10,21);
        string name = "";
        for (int i = 0; i < delkaJmena; ++i)
        {
            name += (char)rnd.Next(97, 123);
        }
        return name;
    }
    private static DateTime VytvorRandomDatumNakupu(Random rnd)
    {
        int rok = rnd.Next(2000, 2019);
        int mesic = rnd.Next(1, 13);
        int den = rnd.Next(1, 29);

        return new DateTime(rok, mesic, den);
    }
    private static DateTime VytvorRandomDatumPosledniRevize(Random rnd)
    {
        int rok = rnd.Next(2019, 2023);
        int mesic = rnd.Next(1, 13);
        int den = rnd.Next(1, 29);

        return new DateTime(rok, mesic, den);
    }
}
