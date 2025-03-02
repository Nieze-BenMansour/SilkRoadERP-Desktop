using bank.Data.Infrastructure;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SystemeService : ISystemeService
    {
        static public DatabaseFactory dbFactory = new DatabaseFactory();
        UnitOfWork utwk = new UnitOfWork(dbFactory);
        public Systeme findById(int id)
        {
            return utwk.getRepository<Systeme>().GetById(id);
        }
        public void editSysteme(Systeme e)
        {
            utwk.getRepository<Systeme>().Update(e);
            utwk.Commit();
        }
    }
    public interface ISystemeService
    {
        Systeme findById(int id);
        void editSysteme(Systeme e);
    }
   
}
