using Ppt23.Shared;
using Mapster;

namespace PPT23.API.Data
{
    public class SeedingData
    {
        private readonly PptDbContext _db;
        private const int PocetVybaveni = 10;

        public SeedingData(PptDbContext db)
        {
            _db = db;
        }

        public async Task SeedData()
        {
            if (!_db.Vybavenis.Any())//není žádné vybavení - nějaké přidáme
            {
                // vytvoř x vybaveních
                //.. přidej do db
                var vybavenis = VybaveniVM.VratRandSeznam(PocetVybaveni, true).Select(x => x.Adapt<Vybaveni>());
                List<Revize> revizes = new();

                _db.Vybavenis.AddRange(vybavenis);

                await _db.SaveChangesAsync();

                var listVybaveniSId = _db.Vybavenis.ToList();

                foreach (Vybaveni v in listVybaveniSId) 
                {
                    int numOfRevizes = Random.Shared.Next(0, 10);
                    for (int i = 0; i < numOfRevizes; i++)
                    {
                        Revize rev = new()
                        {
                            Name = RandomString(Random.Shared.Next(5, 15)),
                            DateTime = v.BoughtDateTime.AddDays(Random.Shared.Next(0, 3 * 365)),
                            VybaveniId = v.Id
                        };
                        v.Revizes.Add(rev);
                        revizes.Add(rev);
                    }
                }

                _db.Revizes.AddRange(revizes);

                await _db.SaveChangesAsync();
            }

        }

        public static string RandomString(int length) =>
           new(Enumerable.Range(0, length).Select(_ =>
           Random.Shared.Next(0, 5) == 0 ? ' ' : //randomly add spaces
           (char)Random.Shared.Next('a', 'z')).ToArray());//add random chars
    }
}
