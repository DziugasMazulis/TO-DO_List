using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Services
{
    public interface IJwtService
    {
        public string GenerateJwt(User user, IList<string> roles);
    }
}
