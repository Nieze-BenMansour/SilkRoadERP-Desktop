using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ProduitService : IProduitService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        public void AddProduit(Produit e)
        {
            utwk.getRepository<Produit>().Add(e);
            utwk.Commit();
        }

        public void deleteProduit( string id)
        {
            utwk.getRepository<Produit>().Delete(utwk.getRepository<Produit>().GetById(id));
            utwk.Commit();
        }

        public void editManyProduit(List<Produit> liste)
        {
       
                 foreach (var item in liste)
                  {
                      utwk.getRepository<Produit>().Update(item);
                  }
                  utwk.Commit();
      
           
        }

        public void editProduit(Produit e)
        {
            utwk.getRepository<Produit>().Update(e);
            utwk.Commit();
        }

        public Produit findProduit(string id)
        {
            return utwk.getRepository<Produit>().GetById(id);
        }

        public List<Produit> findProduitByDesignation(string desig)
        {
            List<Produit> liste_prod = null;
            liste_prod = utwk.getRepository<Produit>().GetMany(t => t.nom.Contains(desig)).ToList();
            return liste_prod;
        }

        public List<Produit> findProduitByRef(string refe)
        {
            List<Produit> liste_prod = null;
            liste_prod = utwk.getRepository<Produit>().GetMany(t => t.refe.Contains(refe)).ToList();
            return liste_prod;
        }

        public Produit findProduitByRefUnique(string refe)
        {
            return utwk.getRepository<Produit>().GetById(refe);
        }

        public List<Produit> getAllProduit()
        {
            return utwk.getRepository<Produit>().GetMany(null, null).ToList();
        }

        public Produit ProduitDetails(string id)
        {
            throw new NotImplementedException();
        }
    }
    public interface IProduitService
    {
        void AddProduit(Produit e);
        List<Produit> getAllProduit();
        Produit findProduit(string id);
        void editProduit(Produit e);
        void editManyProduit(List<Produit> liste);
        void deleteProduit( string id);
        Produit ProduitDetails(string id);
        List<Produit> findProduitByDesignation(string desig);
        List<Produit> findProduitByRef(string refe);

        Produit findProduitByRefUnique(string refe);


    }
}
