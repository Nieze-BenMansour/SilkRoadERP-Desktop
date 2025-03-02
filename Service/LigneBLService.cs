using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LigneBLService : ILigneBLService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddLigneBL(LigneBL e)
        {
            utwk.getRepository<LigneBL>().Add(e);
            utwk.Commit();
        }

        public void AddligneBLMany(List<LigneBL> liste)
        {

                 foreach (var item in liste)
                 {
                     utwk.getRepository<LigneBL>().Add(item);
                 }

                 utwk.Commit();
          
        }

        public void deleteLigneBL(LigneBL e)
        {
            utwk.getRepository<LigneBL>().Delete(e);
            utwk.Commit();
        }

        public void deleteLigneBlByNumBL(BonDeLivraison bl)
        {
            foreach(var item in findLigneBlByNumBL(bl.Num))
            {
                utwk.getRepository<LigneBL>().Delete(item);
            }
            utwk.Commit();
        }

        public List<LigneBL> findLigneBlByNumBL(int num_bl)
        {
            return utwk.getRepository<LigneBL>().GetMany(t => t.Num_BL == num_bl).ToList();
        }
    }
    public interface ILigneBLService
    {
        void AddLigneBL(LigneBL e);
        void AddligneBLMany(List<LigneBL> liste);
        void deleteLigneBL(LigneBL e);
        void deleteLigneBlByNumBL(BonDeLivraison bl);
        List<LigneBL> findLigneBlByNumBL(int num_bl);
    }
}
