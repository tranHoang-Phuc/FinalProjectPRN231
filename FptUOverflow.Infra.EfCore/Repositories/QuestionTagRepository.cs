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
    public class QuestionTagRepository : GenericRepository<QuestionTag>, IQuestionTagRepository
    {
        public QuestionTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
