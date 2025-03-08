using CloudinaryDotNet;
using FptUOverflow.Infra.EfCore.Repositories;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction? _transaction = null;
        private IAnswerRepository _answerRepository;
        private IAnswerVoteRepository _answerVoteRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IQuestionRepository _questionRepository;
        private IQuestionVoteRepository _questionVoteRepository;
        private IQuestionTagRepository _questionTagRepository;
        private ITagRepository _tagRepository;
        private ITagUserRepository _tagUserRepository;
        private ICloudinaryRepository _cloudinaryRepository;
        private IImageUploadRepository _imageUploadRepository;

        public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public AppDbContext Context => _context;

        public IAnswerRepository AnswerRepository => _answerRepository ??= new AnswerRepository(_context);

        public IAnswerVoteRepository AnswerVoteRepository => _answerVoteRepository ??= new AnswerVoteRepository(_context);

        public IApplicationUserRepository ApplicationUserRepository => _applicationUserRepository ??= new ApplicationUserRepository(_context);

        public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);

        public IQuestionVoteRepository QuestionVoteRepository => _questionVoteRepository ??= new QuestionVoteRepository(_context);

        public IQuestionTagRepository QuestionTagRepository => _questionTagRepository ??= new QuestionTagRepository(_context);

        public ITagRepository TagRepository => _tagRepository ??= new TagRepository(_context);

        public ITagUserRepository TagUserRepository => _tagUserRepository ??= new TagUserRepository(_context);

        public ICloudinaryRepository CloudinaryRepository => _cloudinaryRepository ??= new CloudinaryRepository(_serviceProvider);

        public IImageUploadRepository ImageUploadRepository => _imageUploadRepository ??= new ImageUploadRepository(_context);


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction?.CommitAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
