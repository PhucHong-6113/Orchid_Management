using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.JWT
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string username, IEnumerable<string> roles = null);
    }
}
