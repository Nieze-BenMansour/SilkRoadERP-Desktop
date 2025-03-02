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
    public class AvoirFinancierFournisseurService : IAvoirFinancierFournisseurService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        public void AddAvoirFinancierFournisseur(AvoirFinancierFournisseur e)
        {
            utwk.getRepository<AvoirFinancierFournisseur>().Add(e);
            utwk.Commit();
        }

        public void deleteAvoirFinancierFournisseur(AvoirFinancierFournisseur e)
        {
            utwk.getRepository<AvoirFinancierFournisseur>().Delete(e);
            utwk.Commit();
        }

        public void editAvoirFinancierFournisseur(AvoirFinancierFournisseur e)
        {
            utwk.getRepository<AvoirFinancierFournisseur>().Update(e);
            utwk.Commit();
        }

        public AvoirFinancierFournisseur findAvoirFinancierFournisseurByNum(int id)
        {
            AvoirFinancierFournisseur av = new AvoirFinancierFournisseur();
            av = utwk.getRepository<AvoirFinancierFournisseur>().GetById(id);

            return av;
        }

        public AvoirFinancierFournisseur findAvoirFinancierFournisseurByNumFactureFour(FactureFournisseur Fac)
        {
            return utwk.getRepository<AvoirFinancierFournisseur>().Get(t => t.Num == Fac.Num);
        }

        public List<AvoirFinancierFournisseur> getAllAvoirFournisseur()
        {
            return utwk.getRepository<AvoirFinancierFournisseur>().GetManyWithInclude(null, null, t => t.FactureFournisseur).ToList();

        }
    }
    public interface IAvoirFinancierFournisseurService
    {
        void AddAvoirFinancierFournisseur(AvoirFinancierFournisseur e);
        List<AvoirFinancierFournisseur> getAllAvoirFournisseur();
        AvoirFinancierFournisseur findAvoirFinancierFournisseurByNum(int id);
        AvoirFinancierFournisseur findAvoirFinancierFournisseurByNumFactureFour(FactureFournisseur Fac);
        void editAvoirFinancierFournisseur(AvoirFinancierFournisseur e);
        void deleteAvoirFinancierFournisseur(AvoirFinancierFournisseur e);
    }
}
