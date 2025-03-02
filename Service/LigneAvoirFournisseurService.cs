using bank.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;

namespace Service
{
   public class LigneAvoirFournisseurService : ILigneAvoirFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddLigneAvoirFr(LigneAvoirFourniseur e)
        {
            utwk.getRepository<LigneAvoirFourniseur>().Add(e);
            utwk.Commit();
        }

        public void AddligneAvoirFrMany(List<LigneAvoirFourniseur> liste)
        {

            foreach (var item in liste)
            {
                utwk.getRepository<LigneAvoirFourniseur>().Add(item);
            }

            utwk.Commit();

        }

        public void deleteLigneAvoirFr(LigneAvoirFourniseur e)
        {
            utwk.getRepository<LigneAvoirFourniseur>().Delete(e);
            utwk.Commit();
        }

        public void deleteLigneAvoirFourByNumAvoirFr(AvoirFournisseur av)
        {
            foreach (var item in findLigneAvoirFrByNumAvoirFr(av.Num))
            {
                utwk.getRepository<LigneAvoirFourniseur>().Delete(item);
            }
            utwk.Commit();
        }

        public List<LigneAvoirFourniseur> findLigneAvoirFrByNumAvoirFr(int num_avFr)
        {
            return utwk.getRepository<LigneAvoirFourniseur>().GetMany(t => t.Num_avoirFr == num_avFr).ToList();
        }
    }
    public interface ILigneAvoirFournisseurService
    {
        void AddLigneAvoirFr(LigneAvoirFourniseur e);
        void AddligneAvoirFrMany(List<LigneAvoirFourniseur> liste);
        void deleteLigneAvoirFr(LigneAvoirFourniseur e);
        void deleteLigneAvoirFourByNumAvoirFr(AvoirFournisseur av);
        List<LigneAvoirFourniseur> findLigneAvoirFrByNumAvoirFr(int num_avFr);
    }
}

