using System;

// alt sipky, ctrl c

int cisloHadane = NahodneCislo();
int cisloUzivatele = 0;
string konec = "konec";
string? inputNull = "";
string input= "";
int NahodneCislo() => Random.Shared.Next(1, 101);

while (true)
{
    Console.WriteLine("Zkus uhodnout číslo: ");

    try
    {
        inputNull = Console.ReadLine();
        if (inputNull != null)
        {
            input = inputNull;
        }
        else
        {
            throw new Exception();
        }
        cisloUzivatele = Int32.Parse(input);
    }
    catch
    {
        if (konec.Equals(input))
        {
            return;
        }
        Console.WriteLine("Nespravne zadane cislo. Zkus to znovu");
    }

    if (cisloUzivatele == cisloHadane)
    {
        Console.WriteLine("Spravně! Dobrá práce.");
        
        while (true)
        {
            Console.WriteLine("Chceš hrát znovu? ano/ne");
            string? odpovedNull = Console.ReadLine();
            string odpoved = "";
            if (odpovedNull != null)
            {
                odpoved = odpovedNull;
                if (odpoved.Equals("ano"))
                {
                    cisloHadane = NahodneCislo();
                    break;
                }
                if (odpoved.Equals("ne"))
                {
                    return;
                }
            }
            Console.WriteLine("spatna odpoved zkus to znovu.");
        }

    }
    else if (cisloUzivatele > cisloHadane)
    {
        Console.WriteLine("Méně");
    }
    else
    {
        Console.WriteLine("Více");
    }
}