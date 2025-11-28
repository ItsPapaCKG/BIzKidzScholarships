using AutoMapper;
using BizKidzScholarships.Data.Contexts;
using BizKidzScholarships.Data.Entities;
using BizKidzScholarships.Data.NetworkedModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BizKidzScholarships.API.Services.Base
{
    public class BaseCRUDService
    {
        protected ICurrentUser _user;
        protected BizKidzDbContext _context;
        protected IMapper _mapper;
        protected IHttpClientFactory _httpClientFactory;

        public BaseCRUDService(ICurrentUser user, IMapper mapper, BizKidzDbContext context, IHttpClientFactory _fac)
        {
            _user = user;
            _context = context;
            _mapper = mapper;
            _httpClientFactory = _fac;
        }

        protected async Task<bool> SafeUpdateAsync <T> (T entity) where T : class
        {
            using (var t = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().Update(entity);

                    await _context.SaveChangesAsync();

                    await t.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    // TODO post error
                    return false;
                }
            }
        }

        
    }
}
