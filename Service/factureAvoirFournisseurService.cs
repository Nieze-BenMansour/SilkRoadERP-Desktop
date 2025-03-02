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
    public class factureAvoirFournisseurService : IfactureAvoirFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        SystemeService ser_sys = new SystemeService();

        public void AddFactureAvoirFournisseur(FactureAvoirFournisseur e)
        {
            utwk.getRepository<FactureAvoirFournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteFactureAvoirFournisseur(FactureAvoirFournisseur e)
        {
            utwk.getRepository<FactureAvoirFournisseur>().Delete(e);
            utwk.Commit();
        }

        public void editFactureAvoirFournisseur(FactureAvoirFournisseur e)
        {
            utwk.getRepository<FactureAvoirFournisseur>().Update(e);
            utwk.Commit();
        }

        public List<AvoirFournisseur> findAvoirFrBynumFactureAvoirFournisseur(FactureAvoirFournisseur facture)
        {
            if (facture != null)
            {
                List<LigneAvoirFourniseur> listeDestotalLigne = new List<LigneAvoirFourniseur>();
                List<AvoirFournisseur> listeRetour = new List<AvoirFournisseur>();
                listeRetour = utwk.getRepository<AvoirFournisseur>().GetManyWithInclude(t => t.Num_FactureAvoirFournisseur == facture.Num, null, t1 => t1.Fournisseur).ToList();
                foreach (var item in listeRetour)
                {
                    listeDestotalLigne = findLigneFactureAvoirFournisseurByAvoirFr(item);
                    item.tot_H_tva = listeDestotalLigne.Sum(t => t.tot_HT);
                    item.net_payer = listeDestotalLigne.Sum(t => t.tot_TTC);
                    item.tot_tva = listeDestotalLigne.Sum(t => t.tot_TTC) - listeDestotalLigne.Sum(t => t.tot_HT);
                }
                return listeRetour;
            }
            else
                return null;
        }

        public List<FactureAvoirFournisseur> findFactureAvoirFournisseurByFournisseur(Fournisseur four)
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<AvoirFournisseur> listeDestotal;
            List<FactureAvoirFournisseur> listeAretour;
            listeAretour = utwk.getRepository<FactureAvoirFournisseur>().GetManyWithInclude(t => t.id_fournisseur == four.id, null, t1 => t1.Fournisseur).ToList();

            foreach (var item in listeAretour)
            {
                listeDestotal = findAvoirFrBynumFactureAvoirFournisseur(item);
                item.tot_H_tva = listeDestotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDestotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDestotal.Sum(t => t.net_payer) - timbre;
            }
            return listeAretour;
        }

        public FactureAvoirFournisseur findFactureAvoirFournisseurByNum(int id)
        {
            return utwk.getRepository<FactureAvoirFournisseur>().GetById(id);
        }
        public List<LigneAvoirFourniseur> findLigneFactureAvoirFournisseurByAvoirFr(AvoirFournisseur avFR)
        {
            return utwk.getRepository<LigneAvoirFourniseur>().GetMany(t => t.Num_avoirFr == avFR.Num).ToList();
        }

        public List<FactureAvoirFournisseur> getAllFactureAvoirFournisseur()
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<AvoirFournisseur> listeDestotal = new List<AvoirFournisseur>();
            List<FactureAvoirFournisseur> listeAretour = new List<FactureAvoirFournisseur>();
            listeAretour = utwk.getRepository<FactureAvoirFournisseur>().GetManyWithInclude(null, null, t => t.Fournisseur).ToList();
            foreach (var item in listeAretour)
            {
                listeDestotal = findAvoirFrBynumFactureAvoirFournisseur(item);
                item.tot_H_tva = listeDestotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDestotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDestotal.Sum(t => t.net_payer) - timbre;
            }
            return listeAretour;
        }

        public FactureAvoirFournisseur findFactureAvoirFournisseurByNumFactureFour(FactureFournisseur facFour)
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            FactureAvoirFournisseur item = utwk.getRepository<FactureAvoirFournisseur>().Get(y => y.Num_FactureFournisseur == facFour.Num);
            if (item != null)
            {
                var listeDestotal = findAvoirFrBynumFactureAvoirFournisseur(item);
                item.tot_H_tva = listeDestotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDestotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDestotal.Sum(t => t.net_payer) - timbre;
                return item;
            }
            else return null;
        }
    }
    public interface IfactureAvoirFournisseurService
    {
        void AddFactureAvoirFournisseur(FactureAvoirFournisseur e);
        List<FactureAvoirFournisseur> getAllFactureAvoirFournisseur();
        FactureAvoirFournisseur findFactureAvoirFournisseurByNum(int id);
        void editFactureAvoirFournisseur(FactureAvoirFournisseur e);
        void deleteFactureAvoirFournisseur(FactureAvoirFournisseur e);
        List<AvoirFournisseur> findAvoirFrBynumFactureAvoirFournisseur(FactureAvoirFournisseur facture);
        List<FactureAvoirFournisseur> findFactureAvoirFournisseurByFournisseur(Fournisseur four);
        List<LigneAvoirFourniseur> findLigneFactureAvoirFournisseurByAvoirFr(AvoirFournisseur avFR);
        FactureAvoirFournisseur findFactureAvoirFournisseurByNumFactureFour(FactureFournisseur facFour);

    }
}
