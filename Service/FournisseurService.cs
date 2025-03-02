using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
   
    public class FournisseurService : IFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

     
        public void AddFournisseur(Fournisseur e)
        {
            utwk.getRepository<Fournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteFournisseur(long id)
        {
            utwk.getRepository<Fournisseur>().Delete(utwk.getRepository<Fournisseur>().GetById(id));
            utwk.Commit();
        }

        public void editFournisseur(Fournisseur e)
        {
            utwk.getRepository<Fournisseur>().Update(e);
            utwk.Commit();
        }

        public Fournisseur findFournisseurByID(long id)
        {
            return utwk.getRepository<Fournisseur>().GetById(id);
        }

        public List<Fournisseur> findFournisseurByNom(string nom)
        {
            return utwk.getRepository<Fournisseur>().GetMany(t => t.nom.Contains(nom)).ToList();
        }

     

        public List<Fournisseur> getAllFournisseur()
        {
            return utwk.getRepository<Fournisseur>().GetMany(null,null).ToList();
        }
    }
    public interface IFournisseurService
    {
        void AddFournisseur(Fournisseur e);
        List<Fournisseur> getAllFournisseur();
        Fournisseur findFournisseurByID(long id);
        void editFournisseur(Fournisseur e);
        void deleteFournisseur(long id);
        List<Fournisseur> findFournisseurByNom(string nom);
    }
}
