using System.Collections.Generic;
using System.Threading.Tasks;
using ToShowDoc.Core.Config;

namespace ToShowDoc.Core.ShowDoc
{
    public interface IShowDocStore
    {
        Task AddShowDocProject(ShowDocProject project);

        Task UpdateShowDocProject(ShowDocProject project);

        Task<List<ShowDocProject>> GetAll();

        Task DeleteShowDocProject(int id);
    }
}
