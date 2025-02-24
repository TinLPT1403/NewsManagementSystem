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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly NewsContext newsContext;
        public TagRepository(NewsContext context) : base(context)
        {
            newsContext = context;
        }


    }
}
