using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NewsTagRepository : GenericRepository<NewsTag>, INewsTagRepository
    {
        private readonly NewsContext newsContext;
        public NewsTagRepository(NewsContext context) : base(context)
        {
            newsContext = context;
        }
    }
}
