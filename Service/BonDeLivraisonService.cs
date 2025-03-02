using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Service
{
    public class BonDeLivraisonService : IBonDeLivraisonService
    {
        static public DatabaseFactory dbFactory =new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);


        public void AddBonDeLivraison(BonDeLivraison e)
        {
            
            utwk.getRepository<BonDeLivraison>().Add(e);
            utwk.Commit();
            dbFactory.DataContext.Entry(e).Reload();
        }

        public void deleteBonDeLivraison(BonDeLivraison e)
        {
            utwk.getRepository<BonDeLivraison>().Delete(e);
            utwk.Commit();
        }

        public void editBonDeLivraison(BonDeLivraison e)
        {
            utwk.getRepository<BonDeLivraison>().Update(e);
            utwk.Commit();
            dbFactory.DataContext.Entry(e).Reload();

        }

        public List<BonDeLivraison> findBonDeLivraisonByClientNonFacturer(Client client)
        {
           // List<BonDeLivraison> listeRetour = new List<BonDeLivraison>();
          //  listeRetour = 
                return utwk.getRepository<BonDeLivraison>().GetManyWithInclude(t => t.clientId == client.Id && t.Num_Facture == null, null, t1 => t1.Client).ToList();
          /*  foreach (var item in listeRetour)
            {
                item.Client = findClientByID((long)item.clientId);
            }*/
          //  return listeRetour;
           
        }

        public BonDeLivraison findBonDeLivraisonByNum(int id)
        {
            BonDeLivraison bon = new BonDeLivraison();
            bon = utwk.getRepository<BonDeLivraison>().GetById(id);
            bon.Client = findClientByID((long)bon.clientId);

            return bon;
        }

        public List<BonDeLivraison> findBonDeLivraisonBynumFacture(Facture facture)
        {
            List<BonDeLivraison> listeRetour = new List<BonDeLivraison>();
            listeRetour = utwk.getRepository<BonDeLivraison>().GetManyWithInclude(t => t.Num_Facture == facture.Num,null,t1 => t1.Client ).ToList();
          /*  foreach (var item in listeRetour)
            {
                item.Client = findClientByID((long)item.clientId);
            }*/
            return listeRetour;
           
        }

        public List<BonDeLivraison> getAllBonDeLivraison()
        {
             return utwk.getRepository<BonDeLivraison>().GetManyWithInclude(null, null, t=> t.Client).OrderBy(t => t.Num).ToList();
        }
        public Client findClientByID(long id)
        {
            return utwk.getRepository<Client>().GetById(id);
        }

        public long MaxIdBL()
        {
            return utwk.getRepository<BonDeLivraison>().GetMany(null, null).Max(t => t.Num);
        }

   /*     public List<BonDeLivraison> FindByMultipleCritere(DateTime dateDebut, DateTime dateFin, long idClient, int facturer)
        {
            System.Linq.Expressions.Expression<Func<BonDeLivraison,bool>> exDate = t => t.date > dateDebut && t.date < dateFin;
            System.Linq.Expressions.Expression<Func<BonDeLivraison, bool>> exClient = t => t.clientId == idClient;

            utwk.getRepository<BonDeLivraison>().GetMany(exDate);
           if (dateDebut != null && dateFin != null) { }
           if(idClient != -1) { }
           if(facturer != -2) { }
        }*/
    }
    public interface IBonDeLivraisonService
    {
        void AddBonDeLivraison(BonDeLivraison e);
        List<BonDeLivraison> getAllBonDeLivraison();
        BonDeLivraison findBonDeLivraisonByNum(int id);
        void editBonDeLivraison(BonDeLivraison e);
        void deleteBonDeLivraison(BonDeLivraison e);
        Client findClientByID(long id);
        List<BonDeLivraison> findBonDeLivraisonByClientNonFacturer(Client client);
        List<BonDeLivraison> findBonDeLivraisonBynumFacture(Facture facture);
        long MaxIdBL();

     //   List<BonDeLivraison> FindByMultipleCritere(DateTime dateDebut,DateTime dateFin,long idClient , int facturer);

    }

}
