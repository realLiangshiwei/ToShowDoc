using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToShowDoc.Core.Entity;

namespace ToShowDoc.Core.ShowDoc
{
    public interface IShowDocStore
    {
        Task<ShowDocEntity> AddShowDoc(ShowDocEntity entity);

        Task UpdateShowDoc(ShowDocEntity entity);

        Task<List<ShowDocEntity>> GetAll();

        Task DeleteShowDoc(int id);

        Task DeleteShowDoc(Expression<Func<ShowDocEntity, bool>> predicate);
    }
}
