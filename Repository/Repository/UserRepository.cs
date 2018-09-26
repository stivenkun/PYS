using Microsoft.AspNetCore.Identity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UserRepository : GenericRepository<IdentityUser>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {

        }
    }
}
