using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain;

namespace WebApplication1.GraphQL
{
    public class GraphQLQuery
    {
        private readonly DbContext _dbContext;

        public GraphQLQuery(DbContext context)
        {
            _dbContext = context;
        }

        public IQueryable<Recipe> Recipes
        {
            get
            {
                return _dbContext.Set<Recipe>().Include(x => x.Parameters);
            }
        }
        public IQueryable<Parameter> Parameters
        {
            get
            {
                return _dbContext.Set<Parameter>();
            }
        }
     }
}
