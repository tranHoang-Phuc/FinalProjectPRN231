using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Models;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Repositories
{
    public class TagUserRepository : GenericRepository<TagUser>, ITagUserRepository
    {
        public TagUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
