using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DeviService : IDeviService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

  
        public void addDevis(Devi devi)
        {
            utwk.getRepository<Devi>().Add(devi);
            utwk.Commit();
        }

        public Devi findDeviById(long id)
        {
            Devi de = new Devi();
            de = utwk.getRepository<Devi>().GetById(id);
            de.Client = findClientByID(de.id_client);
            return de;
        }

        public List<Devi> getAllDevis()
        {
            return utwk.getRepository<Devi>().GetManyWithInclude(null,null, t => t.Client).OrderBy(t => t.Num).ToList();
        
        }
        public List<Devi> getLast50Devis()
        {
            return utwk.getRepository<Devi>().GetMany(null, null).OrderBy(t => t.Num).Take(50).ToList();
        }
        public Client findClientByID(long id)
        {
            return utwk.getRepository<Client>().GetById(id);
        }

        public long MaxIdDevis()
        {
            return utwk.getRepository<Devi>().GetMany(null, null).Max(t => t.Num);
        }

        public void editDevis(Devi devi)
        {
            utwk.getRepository<Devi>().Update(devi);
            utwk.Commit();
        }
    }
    public interface IDeviService
    {
        void addDevis(Devi devi);
        void editDevis(Devi devi);
        Devi findDeviById(long id);
        List<Devi> getAllDevis();
        Client findClientByID(long id);
        long MaxIdDevis();
        List<Devi> getLast50Devis();
    }
}
