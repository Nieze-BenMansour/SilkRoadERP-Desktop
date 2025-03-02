using bank.Data.Infrastructure;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LigneAvoirService : ILigneAvoirService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddLigneAvoir(LigneAvoir e)
        {
            utwk.getRepository<LigneAvoir>().Add(e);
            utwk.Commit();
        }

        public void AddManyLigneAvoir(List<LigneAvoir> liste)
        {
            foreach(var item in liste)
            {
                utwk.getRepository<LigneAvoir>().Add(item);               
            }
            utwk.Commit();
        }

        public void deleteLigneAvoir(LigneAvoir e)
        {
            utwk.getRepository<LigneAvoir>().Delete(e);
            utwk.Commit();
        }

        public void deleteLigneBlByNumAvoir(Avoir bl)
        {
            foreach (var item in findLigneBlByNumAvoir(bl.Num))
            {
                utwk.getRepository<LigneAvoir>().Delete(item);
            }
            utwk.Commit();
        }

        public List<LigneAvoir> findLigneBlByNumAvoir(int num_av)
        {
            return utwk.getRepository<LigneAvoir>().GetMany(t => t.Num_avoir == num_av).ToList();
        }
    }
    public interface ILigneAvoirService
    {
        void AddLigneAvoir(LigneAvoir e);
        void AddManyLigneAvoir(List<LigneAvoir> liste);
        void deleteLigneAvoir(LigneAvoir e);
        void deleteLigneBlByNumAvoir(Avoir bl);
        List<LigneAvoir> findLigneBlByNumAvoir(int num_av);
    }
}
