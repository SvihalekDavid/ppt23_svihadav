using Microsoft.EntityFrameworkCore;
using Ppt23.Shared;
using System.Collections.Generic;
using Mapster;
using System.Xml.Linq;

namespace PPT23.API.Data
{
    public class PptDbContext : DbContext
    {

        public PptDbContext(DbContextOptions<PptDbContext> options)
            : base(options)
        {

        }

        public DbSet<Vybaveni> Vybavenis => Set<Vybaveni>();
        public DbSet<Revize> Revizes => Set<Revize>();

        public List<VybaveniVM> MakeListVybaveniVM()
        {
            List<VybaveniVM> list = new List<VybaveniVM>();
            VybaveniVM v;

            foreach (Vybaveni item in Vybavenis)
            {
                v = Vybaveni.MakeVybaveniVMFromVybaveni(item);
                list.Add(v);
            }
            return list;
        }

        public List<RevizeViewModel> MakeListRevizeVM()
        {
            List<RevizeViewModel> list = new List<RevizeViewModel>();
            RevizeViewModel r;

            foreach (Revize item in Revizes)
            {
                r = Revize.MakeRevizeVMFromRevize(item);
                list.Add(r);
            }
            return list;
        }

        public VybaveniVM? FindVybaveniVM(Guid id)
        {
            foreach (Vybaveni item in Vybavenis)
            {
                if (id == item.Id)
                {
                    return item.Adapt<VybaveniVM>();
                    //return Vybaveni.MakeVybaveniVMFromVybaveni(item);
                }
            }
            return null;
        }
        public List<RevizeViewModel> FindRevizeVM(string name)
        {
            List<RevizeViewModel> listR = new();
            foreach (Revize item in Revizes)
            {
                if (item.Name.Contains(name))
                {
                    listR.Add(item.Adapt<RevizeViewModel>());
                }
            }
            return listR;
        }

        public List<RevizeViewModel> FindRevizeByVybaveniId(Guid id)
        {
            List<RevizeViewModel> listR = new();
            foreach (Revize item in Revizes)
            {
                if (item.VybaveniId == id)
                {
                    listR.Add(item.Adapt<RevizeViewModel>());
                }
            }
            return listR;
        }

        public Vybaveni? FindVybaveni(Guid id)
        {
            foreach (Vybaveni item in Vybavenis)
            {
                if (id == item.Id)
                {
                    return item;
                }
            }
            return null;
        }

        public VybaveniSrevizemaVM? FindVybaveniSRevizema(Guid id)
        {
            foreach (Vybaveni item in Vybavenis)
            {
                if (id == item.Id)
                {
                    VybaveniSrevizemaVM vr = item.Adapt<VybaveniSrevizemaVM>();
                    vr.Revizes.Clear();
                    foreach (Revize r in Revizes)
                    {
                        if (r.VybaveniId == vr.Id)
                        {
                            vr.Revizes.Add(r.Adapt<RevizeViewModel>());
                        }
                    }
                    return vr;
                }
            }
            return null;
        }

        public void UpdateVybaveni(VybaveniVM prichoziModel)
        {
            foreach (Vybaveni item in Vybavenis)
            {
                if (prichoziModel.Id == item.Id)
                {
                    item.Name = prichoziModel.Name;
                    item.BoughtDateTime = prichoziModel.BoughtDateTime;
                    item.Cena = prichoziModel.Cena;
                }
            }
        }
    }
}
