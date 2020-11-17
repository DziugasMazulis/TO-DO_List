using System.Collections.Generic;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Services
{
    public interface IJwtService
    {
        string GenerateJwt(User user, IList<string> roles);
    }
}
