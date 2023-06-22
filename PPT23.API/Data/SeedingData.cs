using Ppt23.Shared;
using Mapster;
using Bogus;

namespace PPT23.API.Data
{
    public class SeedingData
    {
        private readonly PptDbContext _db;

        public SeedingData(PptDbContext db)
        {
            _db = db;
        }

        public async Task SeedData(bool isDevelopment)
        {
            if (!_db.Vybavenis.Any())//není žádné vybavení - nějaké přidáme
            {
                var faker = new Faker("cz");
                int PocetVybaveni = isDevelopment ? faker.Random.Number(5, 10) : faker.Random.Number(10, 15);
                // vytvoř x vybaveních
                //.. přidej do db
                //var vybavenis = VybaveniVM.VratRandSeznam(PocetVybaveni, true).Select(x => x.Adapt<Vybaveni>());
                List<Vybaveni> vybavenis = new();
                List<Revize> revizes = new();
                List<Ukon> ukons = new();
                List<Pracovnik> pracovniks = new();

                for (int i = 0; i < PocetVybaveni; i++)
                {
                    var v = new Faker<Vybaveni>()
                    .RuleFor(u => u.Id, f => Guid.Empty)
                    .RuleFor(u => u.Name, f => f.Random.String(faker.Random.Number(5, 15), 'a', 'z'))
                    .RuleFor(u => u.BoughtDateTime, f => f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2019, 12, 31)))
                    .RuleFor(u => u.Cena, f => f.Random.Number(0, 100000));

                    vybavenis.Add(v);
                }

                _db.Vybavenis.AddRange(vybavenis);

                await _db.SaveChangesAsync();

                var listVybaveniSId = _db.Vybavenis.ToList();

                int numOfPracovniks = faker.Random.Number(5, 10);  // Random.Shared.Next(5, 10);

                var occupations = new string[] { "Chirurg", "Zdravotni sestra", "Biomedicinsky technik", "IT pracovnik", "Doktor", "Inspektor"};

                for (int i = 0; i < numOfPracovniks; i++)
                {
                    var p = new Faker<Pracovnik>("cz")
                        .RuleFor(u => u.Name, f => f.Name.FullName())
                        .RuleFor(u => u.Povolani, f => f.PickRandom(occupations));
                    //Pracovnik p = new()
                    //{
                    //    Name = RandomString(Random.Shared.Next(5, 15)),
                    //    Povolani = RandomString(Random.Shared.Next(5, 15)),
                    //};
                    pracovniks.Add(p);
                }

                _db.Pracovniks.AddRange(pracovniks);
                await _db.SaveChangesAsync();

                foreach (Vybaveni v in listVybaveniSId) 
                {
                    int numOfRevizes = faker.Random.Number(0, 10); // Random.Shared.Next(0, 10);
                    int numOfUkons= faker.Random.Number(0, 20);  //Random.Shared.Next(0, 20);

                    for (int i = 0; i < numOfRevizes; i++)
                    {
                        var rev = new Faker<Revize>()
                            .RuleFor(u => u.Name, f => f.Random.String(faker.Random.Number(5, 15), 'a', 'z'))
                            .RuleFor(u => u.DateTime, f => f.Date.Between(v.BoughtDateTime, v.BoughtDateTime.AddDays(faker.Random.Number(0, 3*365))))
                            .RuleFor(u => u.VybaveniId, f => v.Id);
                        //Revize rev = new()
                        //{
                        //    Name = RandomString(Random.Shared.Next(5, 15)),
                        //    DateTime = v.BoughtDateTime.AddDays(Random.Shared.Next(0, 3 * 365)),
                        //    VybaveniId = v.Id
                        //};
                        v.Revizes.Add(rev);
                        revizes.Add(rev);
                    }

                    for (int i = 0; i < numOfUkons; i++)
                    {
                        var u = new Faker<Ukon>()
                            .RuleFor(u => u.Kod, f => f.Random.String(faker.Random.Number(5, 15), 'a', 'z'))
                            .RuleFor(u => u.DateTime, f => f.Date.Between(v.BoughtDateTime, v.BoughtDateTime.AddDays(faker.Random.Number(0, 3 * 365))))
                            .RuleFor(u => u.Detail, f => f.Lorem.Sentence())
                            .RuleFor(u => u.VybaveniId, f => v.Id)
                            .RuleFor(u => u.PracovnikId, f => PridejPracovnikaNeboNull());
                        //Ukon u = new()
                        //{
                        //    Kod = RandomString(Random.Shared.Next(5, 10)),
                        //    DateTime = v.BoughtDateTime.AddDays(Random.Shared.Next(0, 3 * 365)),
                        //    Detail = RandomString(Random.Shared.Next(5, 350)),
                        //    VybaveniId = v.Id,
                        //    PracovnikId = PridejPracovnikaNeboNull()
                        //};
                        v.Ukons.Add(u);
                        ukons.Add(u);
                    }

                }

                _db.Revizes.AddRange(revizes);
                _db.Ukons.AddRange(ukons);

                await _db.SaveChangesAsync();
            }

        }

        public Guid? PridejPracovnikaNeboNull()
        {
            int num = Random.Shared.Next(1, 6);
            if (num == 1)
            {
                return null;
            }
            int pocetPracovniku = _db.Pracovniks.Count();
            int pracovnikNum = Random.Shared.Next(0, pocetPracovniku);
            return _db.Pracovniks.ToList()[pracovnikNum].Id;
        }

        public static string RandomString(int length) =>
           new(Enumerable.Range(0, length).Select(_ =>
           Random.Shared.Next(0, 5) == 0 ? ' ' : //randomly add spaces
           (char)Random.Shared.Next('a', 'z')).ToArray());//add random chars
    }
}
