using Covid19Back.DTO;
using Covid19Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Back.Abstract
{
    public interface IJWTTokenService
    {
        string CreateToken(DbUser user);
        string CreateRefreshToken(DbUser user);
        Task<TokensDTO> RefreshAuthToken(string oldAuthToken, string refreshToken);
    }
}
