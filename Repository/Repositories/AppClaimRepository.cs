using AccessData.DataStore;
using AccessData.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class AppClaimRepository : GenericRepository<AppClaims>, IAppClaimRepository
    {
        public AppClaimRepository(ConfigurationStoreContext context) : base(context)
        {

        }
    }
}
