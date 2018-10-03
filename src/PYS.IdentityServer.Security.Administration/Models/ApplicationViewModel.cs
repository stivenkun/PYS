using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PYS.IdentityServer.Security.Administration.Models
{
    public class ApplicationViewModel
    {
        public AccessData.Models.Aplication Application { set; get; }
        public AccessData.Models.AppClaims AppClaims { set;get }
    }
}
