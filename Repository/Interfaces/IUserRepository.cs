﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Identity;
namespace Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<IdentityUser>
    {
    }
}
