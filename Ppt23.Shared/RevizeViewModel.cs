using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ppt23.Shared
{
    public class RevizeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public RevizeViewModel()
        {
            this.Id = Guid.NewGuid();
            this.Name = "";
        }

        public RevizeViewModel(Guid id, string Name)
        {
            this.Id = id;
            this.Name = Name;
        }

        public static List<RevizeViewModel> VratRandSeznam(int pocet)
        {
            Random rnd = new Random();
            List<RevizeViewModel> listRevizi = new List<RevizeViewModel>();
            for (int i = 0; i < pocet; ++i)
            {
                RevizeViewModel revize = new RevizeViewModel(Guid.NewGuid(), VytvorRandomJmeno());
                listRevizi.Add(revize);
            }

            return listRevizi;
        }

        public static string VytvorRandomJmeno()
        {
            int delkaJmena = Random.Shared.Next(10, 21);
            string name = "";
            for (int i = 0; i < delkaJmena; ++i)
            {
                name += (char)Random.Shared.Next(97, 123);
            }
            return name;
        }
    }
}
