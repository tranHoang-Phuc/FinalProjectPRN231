using CloudinaryDotNet;
using FptUOverflow.Infra.EfCore.Repositories;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.DataAccess
{
    public interface IUnitOfWork
    {
        AppDbContext Context { get; }
        IAnswerRepository AnswerRepository { get; }
        IAnswerVoteRepository AnswerVoteRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IQuestionVoteRepository QuestionVoteRepository { get; }
        IQuestionTagRepository QuestionTagRepository { get; }
        ITagRepository TagRepository { get; }
        ITagUserRepository TagUserRepository { get; }
        ICloudinaryRepository CloudinaryRepository { get; }
        IImageUploadRepository ImageUploadRepository { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task DisposeAsync();
    }
}
