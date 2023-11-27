using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class DBHelpers
    {
        private readonly ApplicationDbContext _context;

        public DBHelpers(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool RecordExistsInDB<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _context.Set<T>().Any(predicate);
        }
    }
}
