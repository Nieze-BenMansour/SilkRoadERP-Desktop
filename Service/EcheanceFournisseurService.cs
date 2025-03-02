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
    public class EcheanceFournisseurService : IEcheanceFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        public void AddEcheanceFournisseur(EcheanceDesFournisseur e)
        {
            utwk.getRepository<EcheanceDesFournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteCEcheanceFournisseur(long id)
        {
            utwk.getRepository<EcheanceDesFournisseur>().Delete(utwk.getRepository<EcheanceDesFournisseur>().GetById(id));
            utwk.Commit();
        }

        public void editEcheanceFournisseur(EcheanceDesFournisseur e)
        {
            utwk.getRepository<EcheanceDesFournisseur>().Update(e);
            utwk.Commit();
        }

        public List<EcheanceDesFournisseur> findEcheanceFournisseurByFournisseur(Fournisseur fouur)
        {
            // return utwk.getRepository<EcheanceDesFournisseur>().GetMany(t => t.fournisseur_id == fouur.id).ToList();
            return null;
        }

        public EcheanceDesFournisseur findEcheanceFournisseurByID(long id)
        {
            EcheanceDesFournisseur ret;
            ret = utwk.getRepository<EcheanceDesFournisseur>().GetById(id);
            ret.fournisseur = findFournisseurByID(ret.fournisseur_id);
            return ret;
        }

        public Fournisseur findFournisseurByID(long id)
        {
            return utwk.getRepository<Fournisseur>().GetById(id);
        }

        public List<EcheanceDesFournisseur> getAllEcheanceFournisseur()
        {
            List<EcheanceDesFournisseur> listeDeRetour = new List<EcheanceDesFournisseur>();

            listeDeRetour = utwk.getRepository<EcheanceDesFournisseur>().GetMany(null, null).ToList();
            foreach(var item in listeDeRetour)
            {
                item.fournisseur = findFournisseurByID(item.fournisseur_id);
            }

            return listeDeRetour;
        }
    }
    public interface IEcheanceFournisseurService
    {
       void AddEcheanceFournisseur(EcheanceDesFournisseur e);
        List<EcheanceDesFournisseur> getAllEcheanceFournisseur();
        EcheanceDesFournisseur findEcheanceFournisseurByID(long id);
        void editEcheanceFournisseur(EcheanceDesFournisseur e);
        void deleteCEcheanceFournisseur(long id);
        List<EcheanceDesFournisseur> findEcheanceFournisseurByFournisseur(Fournisseur fouur);

        Fournisseur findFournisseurByID(long id);
    }
}
