﻿
namespace AuthenticationService.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}