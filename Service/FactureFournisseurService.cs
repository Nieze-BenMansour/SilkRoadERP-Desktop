using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FactureFournisseurService : IFactureFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        SystemeService ser_sys = new SystemeService();
        public void AddFactureFournisseur(FactureFournisseur e)
        {
            utwk.getRepository<FactureFournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteFactureFournisseur(FactureFournisseur e)
        {
            utwk.getRepository<FactureFournisseur>().Delete(e);
            utwk.Commit();
        }

        public void editFactureFournisseur(FactureFournisseur e)
        {
            utwk.getRepository<FactureFournisseur>().Update(e);
            utwk.Commit();
        }

        public List<BonDeReception> findBonDeRceptionBynumFactureFournisseur(FactureFournisseur facture)
        {
            List<LigneBonReception> listeDestotalLigne = new List<LigneBonReception>();
            List<BonDeReception> listeRetour = new List<BonDeReception>();
            listeRetour = utwk.getRepository<BonDeReception>().GetManyWithInclude(t => t.Num_Facture_fournisseur == facture.Num,null,t1 => t1.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                listeDestotalLigne = findLigneBonReceptionByNumBC(item);
                item.tot_H_tva = listeDestotalLigne.Sum(t => t.tot_HT);
                item.net_payer = listeDestotalLigne.Sum(t => t.tot_TTC);
                item.tot_tva = listeDestotalLigne.Sum(t => t.tot_TTC) - listeDestotalLigne.Sum(t => t.tot_HT);
            }
            return listeRetour;
        }

        public List<FactureFournisseur> findFactureFournisseurByFournisseur(Fournisseur four)
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<BonDeReception> listeDestotal = new List<BonDeReception>();
            List<FactureFournisseur> listeAretour = new List<FactureFournisseur>();
            listeAretour = utwk.getRepository<FactureFournisseur>().GetManyWithInclude(t => t.id_fournisseur == four.id,null,t1 => t1.Fournisseur).ToList();
            foreach (var item in listeAretour)
            {
                listeDestotal = findBonDeRceptionBynumFactureFournisseur(item);
                item.tot_H_tva = listeDestotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDestotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDestotal.Sum(t => t.net_payer) + timbre;
            }
            return listeAretour;
        }

        public FactureFournisseur findFactureFournisseurByNum(int id)
        {
            return utwk.getRepository<FactureFournisseur>().GetById(id);
        }
        public List<FactureFournisseur> getAllFactureFournisseur()
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<BonDeReception> listeDestotal = new List<BonDeReception>();
            List<FactureFournisseur> listeAretour = new List<FactureFournisseur>();
            listeAretour = utwk.getRepository<FactureFournisseur>().GetManyWithInclude(null, null,t => t.Fournisseur).ToList();
            foreach (var item in listeAretour)
            {
                listeDestotal = findBonDeRceptionBynumFactureFournisseur(item);
                item.tot_H_tva = listeDestotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDestotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDestotal.Sum(t => t.net_payer) + timbre;
            }
            return listeAretour;
        }
        public List<LigneBonReception> findLigneBonReceptionByNumBC(BonDeReception bon)
        {
            return utwk.getRepository<LigneBonReception>().GetMany(t => t.Num_BonRec == bon.Num).ToList();
        }
    }
    public interface IFactureFournisseurService
    {
        void AddFactureFournisseur(FactureFournisseur e);
        List<FactureFournisseur> getAllFactureFournisseur();
        FactureFournisseur findFactureFournisseurByNum(int id);
        void editFactureFournisseur(FactureFournisseur e);
        void deleteFactureFournisseur(FactureFournisseur e);
        List<BonDeReception> findBonDeRceptionBynumFactureFournisseur(FactureFournisseur facture);
      
        List<FactureFournisseur> findFactureFournisseurByFournisseur(Fournisseur four);
        List<LigneBonReception> findLigneBonReceptionByNumBC(BonDeReception bon);
    }
}
