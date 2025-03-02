using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BonDeReceptionService : IBonDeReceptionService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        

        public void AddBonDeReception(BonDeReception e)
        {
            utwk.getRepository<BonDeReception>().Add(e);
            utwk.Commit();
        }

        public void deleteBonDeReception(BonDeReception e)
        {
            utwk.getRepository<BonDeReception>().Delete(e);
            utwk.Commit();
        }

        public void editBonDeReception(BonDeReception e)
        {
            utwk.getRepository<BonDeReception>().Update(e);
            utwk.Commit();
        }

        public List<BonDeReception> findBonDeReceptionByFournisseurNonFacturer(Fournisseur four)
        {
            List<BonDeReception> listeRetour = new List<BonDeReception>();
            listeRetour = utwk.getRepository<BonDeReception>().GetManyWithInclude(t => t.id_fournisseur == four.id && t.Num_Facture_fournisseur == null,null, t => t.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                item.tot_H_tva = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_HT);
                item.net_payer = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_TTC) ;
                item.tot_tva = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_TTC) - findLigneBonReceptionByNumBC(item).Sum(t => t.tot_HT);
            }
            return listeRetour;
            
        }

        public BonDeReception findBonDeReceptionByNum(int id)
        {
            BonDeReception bon = new BonDeReception();
            bon = utwk.getRepository<BonDeReception>().GetById(id);
            bon.Fournisseur = findFournisseurByID(bon.id_fournisseur);

            bon.tot_H_tva = findLigneBonReceptionByNumBC(bon).Sum(t => t.tot_HT);
            bon.net_payer = findLigneBonReceptionByNumBC(bon).Sum(t => t.tot_TTC);
            bon.tot_tva = findLigneBonReceptionByNumBC(bon).Sum(t => t.tot_TTC) - findLigneBonReceptionByNumBC(bon).Sum(t => t.tot_HT);

            return bon;
        }

        public List<BonDeReception> findBonDeReceptionBynumFactureFour(FactureFournisseur facture)
        {
            List<BonDeReception> listeRetour = new List<BonDeReception>();
            listeRetour = utwk.getRepository<BonDeReception>().GetManyWithInclude(t => t.Num_Facture_fournisseur == facture.Num, null, t => t.Fournisseur).ToList();
            foreach (var item in listeRetour)
            {
                item.tot_H_tva = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_HT);
                item.net_payer = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_TTC);
                item.tot_tva = findLigneBonReceptionByNumBC(item).Sum(t => t.tot_TTC) - findLigneBonReceptionByNumBC(item).Sum(t => t.tot_HT);
            }
            return listeRetour;
            
        }

        public List<BonDeReception> getAllBonDeReception()
        {
            return  utwk.getRepository<BonDeReception>().GetManyWithInclude(null, null, t => t.Fournisseur).ToList();
        }
        public  Fournisseur findFournisseurByID(long id)
        {
            return utwk.getRepository<Fournisseur>().GetById(id);
        }

        public List<LigneBonReception> findLigneBonReceptionByNumBC(BonDeReception bon)
        {
            return utwk.getRepository<LigneBonReception>().GetMany(t => t.Num_BonRec == bon.Num).ToList();
        }

        public long MaxIdBC()
        {
            return utwk.getRepository<BonDeReception>().GetMany(null, null).Max(t => t.Num);
        }
    }
    public interface IBonDeReceptionService
    {
        void AddBonDeReception(BonDeReception e);
        List<BonDeReception> getAllBonDeReception();
        BonDeReception findBonDeReceptionByNum(int id);
        void editBonDeReception(BonDeReception e);
        void deleteBonDeReception(BonDeReception e);
        Fournisseur findFournisseurByID(long id);
        List<BonDeReception> findBonDeReceptionByFournisseurNonFacturer(Fournisseur four);
        List<BonDeReception> findBonDeReceptionBynumFactureFour(FactureFournisseur facture);
        List<LigneBonReception> findLigneBonReceptionByNumBC(BonDeReception bon);

        long MaxIdBC();

    }
}
