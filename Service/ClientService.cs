using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ClientService : IClientService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);

    
        public void AddClient(Client e)
        {
            utwk.getRepository<Client>().Add(e);
            utwk.Commit();
        }

        public void deleteClient( long id)
        {
            utwk.getRepository<Client>().Delete(utwk.getRepository<Client>().GetById(id));
            utwk.Commit();
        }

        public void editClient(Client e)
        {
            utwk.getRepository<Client>().Update(e);
            utwk.Commit();
        }

        public Client findClientByID(long id)
        {
            return utwk.getRepository<Client>().GetById(id);
        }

        public List<Client> findClientByIdContains(long id)
        {
            return utwk.getRepository<Client>().GetMany(t => t.Id.ToString().Contains(id.ToString()) ).ToList();
        }

        public List<Client> findClientByNom(string nom)
        {
            return utwk.getRepository<Client>().GetMany(t => t.nom.Contains(nom)).ToList();
        }

        public List<Client> getAllClient()
        {
            return utwk.getRepository<Client>().GetMany(null, null).ToList();
        }
    }
    public interface IClientService
    {
        void AddClient(Client e);
        List<Client> getAllClient();
        Client findClientByID(long id);
        void editClient(Client e);
        void deleteClient( long id);
        List<Client> findClientByNom(string nom);

        List<Client> findClientByIdContains(long id);
    }
}
