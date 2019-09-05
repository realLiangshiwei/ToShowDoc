using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToShowDoc.Core.Entity;

namespace ToShowDoc.Core.ShowDoc
{
    public class DefaultShowDocStore : IShowDocStore
    {
        private readonly List<ShowDocEntity> _data;

        private readonly IShowDocDataProvider _dataProvider;

        public DefaultShowDocStore(IShowDocDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _data = new List<ShowDocEntity>();
        }

        public async Task<ShowDocEntity> AddShowDoc(ShowDocEntity entity)
        {
            await LoadData();
            if (!_data.Any())
            {
                entity.Id = 1;
            }
            else
            {
                entity.Id = _data.Max(x => x.Id) + 1;
            }

            _data.Add(entity);
            await _dataProvider.SaveChanges(_data);
            return entity;
        }

        public async Task UpdateShowDoc(ShowDocEntity entity)
        {
            await LoadData();
            _data.RemoveAll(x => x.Id == entity.Id);
            _data.Add(entity);
            await _dataProvider.SaveChanges(_data);
        }

        public async Task<List<ShowDocEntity>> GetAll()
        {
            await LoadData();
            return _data;
        }

        public async Task DeleteShowDoc(int id)
        {
            await LoadData();
            _data.RemoveAll(x => x.Id == id);
            await _dataProvider.SaveChanges(_data);
        }

        public async Task DeleteShowDoc(Expression<Func<ShowDocEntity, bool>> predicate)
        {
            await LoadData();
            var showDoc = _data.FirstOrDefault(predicate.Compile());
            await _dataProvider.SaveChanges(_data);
        }

        private async Task LoadData()
        {
            if (_data.Count == 0)
                _data.AddRange(await _dataProvider.LoadData());
        }
    }
}
