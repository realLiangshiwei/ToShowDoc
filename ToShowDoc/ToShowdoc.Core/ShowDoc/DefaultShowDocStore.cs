using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToShowDoc.Core.Config;

namespace ToShowDoc.Core.ShowDoc
{
    public class DefaultShowDocStore : IShowDocStore
    {
        private readonly List<ShowDocProject> _docProjects;

        public DefaultShowDocStore()
        {
            _docProjects = new List<ShowDocProject>();
        }

        public Task AddShowDocProject(ShowDocProject project)
        {
            _docProjects.Add(project);
            return Task.CompletedTask;
        }

        public Task UpdateShowDocProject(ShowDocProject project)
        {
            _docProjects.RemoveAll(x => x.Id == project.Id);
            _docProjects.Add(project);
            return Task.CompletedTask;
        }

        public Task<List<ShowDocProject>> GetAll()
        {
            return Task.FromResult(_docProjects);
        }

        public Task DeleteShowDocProject(int id)
        {
            return Task.FromResult(_docProjects.FirstOrDefault(x => x.Id == id));
        }
    }
}
