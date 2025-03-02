using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LigneBonDeRecepctionService : ILigneBonDeRecepctionService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);


        public void AddLigneBonReception(LigneBonReception e)
        {
            utwk.getRepository<LigneBonReception>().Add(e);
            utwk.Commit();
        }

        public void AddManyLigneBonDeReception(List<LigneBonReception> liste)
        {
            foreach(var item in liste)
            {
                utwk.getRepository<LigneBonReception>().Add(item);
            }
            utwk.Commit();
        }

        public void deleteLigneBonReception(LigneBonReception e)
        {
            utwk.getRepository<LigneBonReception>().Delete(e);
            utwk.Commit();
        }

        public void deleteLigneBonReceptionByNumBC(BonDeReception bc)
        {
            foreach (var item in findLigneBonReceptionByNumBC(bc.Num))
            {
                utwk.getRepository<LigneBonReception>().Delete(item);
            }
            utwk.Commit();
        }

        public List<LigneBonReception> findLigneBonReceptionByNumBC(int num_bc)
        {
            return utwk.getRepository<LigneBonReception>().GetMany(t => t.Num_BonRec == num_bc).ToList();
        }

        public List<LigneBonReception> findLigneBonReceptionByRefProd(string ref_prod)
        {
            List<LigneBonReception> lista = new List<LigneBonReception>();
            lista = utwk.getRepository<LigneBonReception>().GetManyWithInclude(t => t.Ref_Produit == ref_prod, null, g => g.BonDeReception.Fournisseur).OrderByDescending(t => t.BonDeReception.dateDeLivraison).ToList();
            decimal fodec = 0;
            decimal tottmp = 0;
            decimal valeurremise = 0;
            foreach ( var item in lista)
            {
                valeurremise = item.prix_HT * Convert.ToDecimal(item.remise) / 100;
                tottmp = item.prix_HT -   valeurremise ;
                item.NetTtcUnitaire = Math.Round(tottmp + ((tottmp * (decimal)item.tva) / 100));
                if (item.BonDeReception.Fournisseur.constructeur)
                {
                    fodec = item.prix_HT + (item.prix_HT / 100);
                    item.prixHtFodec = fodec;
                    valeurremise = fodec * Convert.ToDecimal(item.remise) / 100;
                    tottmp = fodec - valeurremise;
                    item.NetTtcUnitaire = Math.Round(tottmp + ((tottmp * (decimal)item.tva) / 100));
                }
            }
            return lista;
        }
    }
    public interface ILigneBonDeRecepctionService
    {
        void AddLigneBonReception(LigneBonReception e);
        void AddManyLigneBonDeReception(List<LigneBonReception> liste);
        void deleteLigneBonReception(LigneBonReception e);
        void deleteLigneBonReceptionByNumBC(BonDeReception bc);
        List<LigneBonReception> findLigneBonReceptionByNumBC(int num_bc);
        List<LigneBonReception> findLigneBonReceptionByRefProd(string ref_prod);
    }
}
