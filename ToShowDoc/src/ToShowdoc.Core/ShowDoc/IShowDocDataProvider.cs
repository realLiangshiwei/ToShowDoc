using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToShowDoc.Core.Entity;

namespace ToShowDoc.Core.ShowDoc
{
    public interface IShowDocDataProvider
    {
        Task<List<ShowDocEntity>> LoadData();

        Task SaveChanges(List<ShowDocEntity> data);
    }

}
