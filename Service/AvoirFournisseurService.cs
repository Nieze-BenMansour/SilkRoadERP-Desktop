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
   public class AvoirFournisseurService : IAvoirFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddAvoirFournisseur(AvoirFournisseur e)
        {
            utwk.getRepository<AvoirFournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteAvoirFournisseur(AvoirFournisseur e)
        {
            utwk.getRepository<AvoirFournisseur>().Delete(e);
            utwk.Commit();
        }

        public void editAvoirFournisseur(AvoirFournisseur e)
        {
            utwk.getRepository<AvoirFournisseur>().Update(e);
            utwk.Commit();
        }

        public List<AvoirFournisseur> findAvoirFournisseurByFournisseurNonFacturer(Fournisseur four)
        {
            List<LigneAvoirFourniseur> listeligne;
            List<AvoirFournisseur> listeRetour = new List<AvoirFournisseur>();
            listeRetour = utwk.getRepository<AvoirFournisseur>().GetManyWithInclude(t => t.fournisseurId == four.id && t.Num_FactureAvoirFournisseur == null, null, t1 => t1.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                listeligne = new List<LigneAvoirFourniseur>();
                listeligne = findLigneAvoirFourniseurByNumAvFr(item);
                item.tot_H_tva = listeligne.Sum(t => t.tot_HT);
                item.net_payer = listeligne.Sum(t => t.tot_TTC);
                item.tot_tva = listeligne.Sum(t => t.tot_TTC) - listeligne.Sum(t => t.tot_HT);
            }
            return listeRetour;
        }

        public AvoirFournisseur findAvoirFournisseurByNum(int id)
        {
            AvoirFournisseur av = new AvoirFournisseur();
            av = utwk.getRepository<AvoirFournisseur>().GetById(id);
            av.Fournisseur = findFournisseurByID((long)av.fournisseurId);

            return av;
        }

        public List<AvoirFournisseur> findAvoirFournisseurBynumFacture(FactureAvoirFournisseur factureAvFr)
        {
            List<LigneAvoirFourniseur> listeligne;
            List<AvoirFournisseur> listeRetour = new List<AvoirFournisseur>();
            listeRetour = utwk.getRepository<AvoirFournisseur>().GetManyWithInclude(t => t.Num_FactureAvoirFournisseur == factureAvFr.Num, null, t1 => t1.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                listeligne = new List<LigneAvoirFourniseur>();
                listeligne = findLigneAvoirFourniseurByNumAvFr(item);
                item.tot_H_tva = listeligne.Sum(t => t.tot_HT);
                item.net_payer = listeligne.Sum(t => t.tot_TTC);
                item.tot_tva = listeligne.Sum(t => t.tot_TTC) - listeligne.Sum(t => t.tot_HT);
            }
            return listeRetour;
        }

        public Fournisseur findFournisseurByID(long id)
        {
            return utwk.getRepository<Fournisseur>().GetById(id);
        }

        public List<LigneAvoirFourniseur> findLigneAvoirFourniseurByNumAvFr(AvoirFournisseur avFr)
        {
            return utwk.getRepository<LigneAvoirFourniseur>().GetMany(t => t.Num_avoirFr == avFr.Num).ToList();
        }

        public List<AvoirFournisseur> getAllAvoirFournisseur()
        {
            List<LigneAvoirFourniseur> listeligne;
            List<AvoirFournisseur> listeRetour = new List<AvoirFournisseur>();
            listeRetour = utwk.getRepository<AvoirFournisseur>().GetManyWithInclude(null, null, t => t.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                listeligne = new List<LigneAvoirFourniseur>();
                listeligne = findLigneAvoirFourniseurByNumAvFr(item);
                item.tot_H_tva = listeligne.Sum(t => t.tot_HT);
                item.net_payer = listeligne.Sum(t => t.tot_TTC);
                item.tot_tva = listeligne.Sum(t => t.tot_TTC) - listeligne.Sum(t => t.tot_HT);
            }
            return listeRetour;
        }

        public long MaxIdBL()
        {
            return utwk.getRepository<AvoirFournisseur>().GetMany(null, null).Max(t => t.Num);
        }
    }
    public interface IAvoirFournisseurService
    {
        void AddAvoirFournisseur(AvoirFournisseur e);
        List<AvoirFournisseur> getAllAvoirFournisseur();
        AvoirFournisseur findAvoirFournisseurByNum(int id);
        void editAvoirFournisseur(AvoirFournisseur e);
        void deleteAvoirFournisseur(AvoirFournisseur e);
        Fournisseur findFournisseurByID(long id);
        List<AvoirFournisseur> findAvoirFournisseurByFournisseurNonFacturer(Fournisseur four);
        List<AvoirFournisseur> findAvoirFournisseurBynumFacture(FactureAvoirFournisseur factureAvFr);
        long MaxIdBL();
        List<LigneAvoirFourniseur> findLigneAvoirFourniseurByNumAvFr(AvoirFournisseur avFr);

        //   List<BonDeLivraison> FindByMultipleCritere(DateTime dateDebut,DateTime dateFin,long idClient , int facturer);

    }
}
