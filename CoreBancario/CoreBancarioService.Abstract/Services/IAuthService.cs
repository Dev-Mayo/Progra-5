using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Services
{
    public interface IAuthService
    {
        bool ValidateToken(string token);
    }
}
