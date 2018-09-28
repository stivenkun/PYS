using AccessData.Datas;
using AccessData.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class ApplicationRepository : GenericRepository<Aplication>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
