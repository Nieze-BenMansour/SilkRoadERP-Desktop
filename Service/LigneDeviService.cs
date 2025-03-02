using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LigneDeviService : ILigneDeviService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void addLigneDevi(LigneDevi lig)
        {
            utwk.getRepository<LigneDevi>().Add(lig);
            utwk.Commit();
        }

        public void AddManyLigneDevi(List<LigneDevi> liste)
        {
            foreach(var item in liste)
            {
                utwk.getRepository<LigneDevi>().Add(item);
            }
            utwk.Commit();
        }

        public void DeleteLigneDevisByNumDevis(Devi devi)
        {
            foreach (var item in GetLigneByNumDevis(devi.Num))
            {
                utwk.getRepository<LigneDevi>().Delete(item);
                
            }
            utwk.Commit();
        }

        public List<LigneDevi> GetLigneByNumDevis(long num_devis)
        {
            return utwk.getRepository<LigneDevi>().GetMany(t => t.Num_devis == num_devis).ToList();
        }
    }
    public interface ILigneDeviService
    {
        void addLigneDevi(LigneDevi lig);
        void AddManyLigneDevi(List<LigneDevi> liste);
      List<LigneDevi> GetLigneByNumDevis( long num_devis);
      void DeleteLigneDevisByNumDevis(Devi devi);
    }
}
