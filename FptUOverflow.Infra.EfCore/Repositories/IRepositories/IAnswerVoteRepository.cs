using FptUOverflow.Infra.EfCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Repositories.IRepositories
{
    public interface IAnswerVoteRepository : IGenericRepository<AnswerVote>
    {
    }
}
