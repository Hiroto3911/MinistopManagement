using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork 
    {
        void BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
    }
}
