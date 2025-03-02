using bank.Data.Infrastructure;
using Domain.Entites;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AvoirService : IAvoirService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void addAvoir(Avoir e)
        {
            utwk.getRepository<Avoir>().Add(e);
            utwk.Commit();
        }

        public void deleteAvoir(Avoir e)
        {
            utwk.getRepository<Avoir>().Delete(e);
            utwk.Commit();
        }

        public void editAvoir(Avoir e)
        {
            utwk.getRepository<Avoir>().Update(e);
            utwk.Commit();
        }

        public Avoir findAvoirById(long id)
        {
            Avoir de = new Avoir();
            de = utwk.getRepository<Avoir>().GetById(id);
            de.Client = findClientByID(de.clientId.GetValueOrDefault());
            de.tot_H_tva = findLigneAvoirByNumAvoir(de).Sum(t => t.tot_HT);
            de.net_payer = findLigneAvoirByNumAvoir(de).Sum(t => t.tot_TTC);
            de.tot_tva = findLigneAvoirByNumAvoir(de).Sum(t => t.tot_TTC) - findLigneAvoirByNumAvoir(de).Sum(t => t.tot_HT);
            return de;
        }

        public Client findClientByID(int id)
        {
            return utwk.getRepository<Client>().GetById(id);
        }

        public List<LigneAvoir> findLigneAvoirByNumAvoir(Avoir av)
        {
            return utwk.getRepository<LigneAvoir>().GetMany(t => t.Num_avoir == av.Num).ToList();
        }

        public List<Avoir> getAllAvoirs()
        {
            List<LigneAvoir> listeligne = new List<LigneAvoir>();
            List<Avoir> listeRetour = new List<Avoir>();
            listeRetour = utwk.getRepository<Avoir>().GetManyWithInclude(null, null,t => t.Client).ToList();
            foreach (var item in listeRetour)
            {
                listeligne = findLigneAvoirByNumAvoir(item);
                item.tot_H_tva = listeligne.Sum(t => t.tot_HT);
                item.net_payer = listeligne.Sum(t => t.tot_TTC);
                item.tot_tva = listeligne.Sum(t => t.tot_TTC) - listeligne.Sum(t => t.tot_HT);
            }
            return listeRetour;
        }
    }
    public interface IAvoirService
    {
        void addAvoir(Avoir e);
        void editAvoir(Avoir e);
        void deleteAvoir(Avoir e);
        Avoir findAvoirById(long id);
        List<Avoir> getAllAvoirs();
        Client findClientByID(int id);
        List<LigneAvoir> findLigneAvoirByNumAvoir(Avoir av);
    }
}
