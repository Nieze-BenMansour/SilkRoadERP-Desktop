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
    public class CommandeService : ICommandeService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

        public void AddCommande(Commande e)
        {
            utwk.getRepository<Commande>().Add(e);
            utwk.Commit();
        }

        public void deleteCommande(Commande e)
        {
            utwk.getRepository<Commande>().Delete(e);
            utwk.Commit();
        }

        public void editCommande(Commande e)
        {
            utwk.getRepository<Commande>().Update(e);
            utwk.Commit();
        }

        public Commande findCommandeByNum(long id)
        {
            Commande bon = new Commande();
            bon = utwk.getRepository<Commande>().GetById(id);
            bon.fournisseur = findFournisseurByID(bon.fournisseurId.GetValueOrDefault());

            bon.tot_H_tva = findLigneCommandeByNumCO(bon).Sum(t => t.tot_HT);
            bon.net_payer = findLigneCommandeByNumCO(bon).Sum(t => t.tot_TTC);
            bon.tot_tva = findLigneCommandeByNumCO(bon).Sum(t => t.tot_TTC) - findLigneCommandeByNumCO(bon).Sum(t => t.tot_HT);

            return bon;
        }

        public Fournisseur findFournisseurByID(int id)
        {
            return utwk.getRepository<Fournisseur>().GetById(id);
        }

        public List<LigneCommande> findLigneCommandeByNumCO(Commande com)
        {
            return utwk.getRepository<LigneCommande>().GetMany(t => t.Num_commande == com.Num).ToList();
        }

        public List<Commande> getAllCommande()
        {
            List<Commande> listeRetour = new List<Commande>();
            listeRetour = utwk.getRepository<Commande>().GetMany(null, null).ToList();
            foreach (var item in listeRetour)
            {
                item.fournisseur = findFournisseurByID(item.fournisseurId.GetValueOrDefault());
                item.tot_H_tva = findLigneCommandeByNumCO(item).Sum(t => t.tot_HT);
                item.net_payer = findLigneCommandeByNumCO(item).Sum(t => t.tot_TTC);
                item.tot_tva = findLigneCommandeByNumCO(item).Sum(t => t.tot_TTC) - findLigneCommandeByNumCO(item).Sum(t => t.tot_HT);
            }
            return listeRetour;
        }
    }
    public interface ICommandeService
    {
        void AddCommande(Commande e);
        List<Commande> getAllCommande();
        Commande findCommandeByNum(long id);
        void editCommande(Commande e);
        void deleteCommande(Commande e);
        Fournisseur findFournisseurByID(int id);
        List<LigneCommande> findLigneCommandeByNumCO(Commande com);
    }
}
