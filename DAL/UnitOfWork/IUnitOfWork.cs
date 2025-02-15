using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories {  get; }
        INewsArticleRepository NewsArticles {  get; }
        INewsTagRepository NewsTags { get; }
        ISystemAccountRepository SystemAccounts { get; }
        ITagRepository Tags { get; }
        void Dispose();
        Task<bool> SaveChangesAsync();
        bool SaveChanges();
    }
}
