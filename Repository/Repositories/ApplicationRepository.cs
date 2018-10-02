using AccessData.Datas;
using AccessData.DataStore;
using AccessData.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
//using PYS.IdentityServer.Security.Administration.ConfigurationStore;
namespace Repository.Repositories
{
    public class ApplicationRepository : GenericRepository<Aplication>, IApplicationRepository
    {
        public ApplicationRepository(ConfigurationStoreContext context) : base(context)
        {

        }
    }
}
