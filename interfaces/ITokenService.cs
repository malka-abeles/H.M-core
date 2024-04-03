using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


// namespace myTasks.Services
namespace myTasks.Interfaces
{
    public interface ITaskTokenService
    {
        // private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        // private static string issuer = "https://user-demo.com";
        SecurityToken GetToken(List<Claim> claims);

        TokenValidationParameters GetTokenValidationParameters();
     
        string WriteToken(SecurityToken token) ;
    }
}