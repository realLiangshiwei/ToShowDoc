using System.Collections.Generic;
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
    }
}
