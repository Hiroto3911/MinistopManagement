using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        IReadOnlyList<Promotion> GetAllIsDelete();
        void SoftDelete(Promotion entity, bool hasTransaction = false);
        void SoftDeleteRange(IList<Promotion> entities, bool hasTransaction = false);
    }
}
