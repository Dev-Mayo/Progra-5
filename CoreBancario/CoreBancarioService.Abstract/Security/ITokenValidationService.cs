using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBancarioService.Abstract.Security
{
    public interface ITokenValidationService
    {
        Task<bool> ValidateTokenAsync(string token);
    }
}
