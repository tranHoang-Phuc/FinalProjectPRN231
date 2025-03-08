using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
