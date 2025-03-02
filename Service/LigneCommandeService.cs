using bank.Data.Infrastructure;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LigneCommandeService : ILigneCommandeService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddLigneCommande(LigneCommande e)
        {
            utwk.getRepository<LigneCommande>().Add(e);
            utwk.Commit();
        }

        public void AddManyLigneCommande(List<LigneCommande> liste)
        {
            foreach(var item in liste)
            {
                utwk.getRepository<LigneCommande>().Add(item);
            }
            utwk.Commit();
        }

        public void deleteLigneCommande(LigneCommande e)
        {
            utwk.getRepository<LigneCommande>().Delete(e);
            utwk.Commit();
        }

        public void deleteLigneCommandeByNumCO(Commande co)
        {
            foreach (var item in findLigneCommandeByNumCO(co.Num))
            {
                utwk.getRepository<LigneCommande>().Delete(item);
            }
            utwk.Commit();
        }

        public List<LigneCommande> findLigneCommandeByNumCO(int num_co)
        {
            return utwk.getRepository<LigneCommande>().GetMany(t => t.Num_commande == num_co).ToList();
        }
    }
    public interface ILigneCommandeService
    {
        void AddLigneCommande(LigneCommande e);
        void AddManyLigneCommande(List<LigneCommande> liste);
        void deleteLigneCommande(LigneCommande e);
        void deleteLigneCommandeByNumCO(Commande co);
        List<LigneCommande> findLigneCommandeByNumCO(int num_co);
    }
}
