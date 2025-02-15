using DAL.Data;
using DAL.Entities;
using DAL.Repositories;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsContext _newsContext;
        public UnitOfWork(NewsContext context)
        {
            _newsContext = context;
            Categories = new CategoryRepository(_newsContext);
            NewsArticles = new NewsArticleRepository(_newsContext);
            NewsTags = new NewsTagRepository(_newsContext);
            SystemAccounts = new SystemAccountRepository(_newsContext);
            Tags = new TagRepository(_newsContext);
        }

        public ICategoryRepository Categories { get; private set; } 
        public INewsArticleRepository NewsArticles {  get; private set; }
        public INewsTagRepository NewsTags { get; private set; }
        public ISystemAccountRepository SystemAccounts { get; private set; }
        public ITagRepository Tags { get; private set; }

        public void Dispose()
        {
            _newsContext.Dispose();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _newsContext.SaveChangesAsync() > 0;
        }
        public bool SaveChanges()
        {
            return _newsContext.SaveChanges() > 0;
        }

    }
}
