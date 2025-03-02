using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FactureService : IFactureService
    {
      static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk;// = new UnitOfWork(dbFactory);
        SystemeService ser_sys;
 

        public FactureService()
        {
       //     DatabaseFactory dbFactory = new DatabaseFactory();
            utwk = new UnitOfWork(dbFactory);
            ser_sys = new SystemeService();
           
        }

        public void AddFacture(Facture e)
        {
            utwk.getRepository<Facture>().Add(e);
            utwk.Commit();
        }

        public void deleteFacture(Facture e)
        {
            utwk.getRepository<Facture>().Delete(e);
            utwk.Commit();
        }

        public void editFacture(Facture e)
        {
            utwk.getRepository<Facture>().Update(e);
            utwk.Commit();
        }

        public List<Facture> findFactureByClient(Client client)
        {
            using (var hh = new DatabaseFactory())
            {
            utwk = new UnitOfWork(hh);
            decimal timbre= ser_sys.findById(1).Timbre;
            List<BonDeLivraison> listeDesTotal = new List<BonDeLivraison>();
            List<Facture> listeAretour = new List<Facture>();
            listeAretour= utwk.getRepository<Facture>().GetManyWithInclude(t => t.id_client == client.Id,null,t1 => t1.Client).ToList();
            foreach(var item in listeAretour)
            {
                listeDesTotal = findBonDeLivraisonBynumFacture(item);
                item.tot_H_tva = listeDesTotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDesTotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDesTotal.Sum(t => t.net_payer) + timbre;
            }
            return listeAretour;
            }
        }
        public List<Facture> findLast40FactureByClient(Client client)
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<BonDeLivraison> listeDesTotal = new List<BonDeLivraison>();
            List<Facture> listeAretour = new List<Facture>();
            listeAretour = utwk.getRepository<Facture>().GetMany(t => t.id_client == client.Id ).Reverse<Facture>().Take(15).OrderBy(t => t.Num).ToList();
            foreach (var item in listeAretour)
            {
                listeDesTotal = findBonDeLivraisonBynumFacture(item);
                item.tot_H_tva = listeDesTotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDesTotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDesTotal.Sum(t => t.net_payer) + timbre;
            }
            return listeAretour;
        }
        //a reviser les tot
        public Facture findFactureByNum(int id)
        {
        
               
                decimal timbre = ser_sys.findById(1).Timbre;
                List<BonDeLivraison> listeDesTotal = new List<BonDeLivraison>();
                Facture elementDeRetour = new Facture();
                elementDeRetour = utwk.getRepository<Facture>().GetById(id);
                listeDesTotal = findBonDeLivraisonBynumFacture(elementDeRetour);
                elementDeRetour.tot_H_tva = listeDesTotal.Sum(t => t.tot_H_tva);
                elementDeRetour.tot_tva = listeDesTotal.Sum(t => t.tot_tva);
                elementDeRetour.tot_ttc = listeDesTotal.Sum(t => t.net_payer) + timbre;

                return elementDeRetour;
            
        }

        public List<Facture> getAllFacture()
        {
            decimal timbre = ser_sys.findById(1).Timbre;
            List<BonDeLivraison> listeDesTotal = new List<BonDeLivraison>();
            List<Facture> listeAretour = new List<Facture>();
            listeAretour= utwk.getRepository<Facture>().GetManyWithInclude(null, null, t => t.Client).OrderBy(t => t.Num).ToList();
            foreach (var item in listeAretour)
            {
                listeDesTotal = findBonDeLivraisonBynumFacture(item);
                item.tot_H_tva = listeDesTotal.Sum(t => t.tot_H_tva);
                item.tot_tva = listeDesTotal.Sum(t => t.tot_tva);
                item.tot_ttc = listeDesTotal.Sum(t => t.net_payer) + timbre;
            }
            return listeAretour;
        }
        public List<BonDeLivraison> findBonDeLivraisonBynumFacture(Facture facture)
        {
            List<BonDeLivraison> listeRetour = new List<BonDeLivraison>();
            listeRetour = utwk.getRepository<BonDeLivraison>().GetManyWithInclude(t => t.Num_Facture == facture.Num,null,t1 => t1.Client).ToList();
           
            return listeRetour;

        }
        public Client findClientByID(long id)
        {
            return utwk.getRepository<Client>().GetById(id);
        }
    }
    public interface IFactureService
    {
        void AddFacture(Facture e);
        List<Facture> getAllFacture();
        Facture findFactureByNum(int id);
        void editFacture(Facture e);
        void deleteFacture(Facture e);
        List<BonDeLivraison> findBonDeLivraisonBynumFacture(Facture facture);
        Client findClientByID(long id);
        List<Facture> findFactureByClient(Client client);
        List<Facture> findLast40FactureByClient(Client client);

    }
}
