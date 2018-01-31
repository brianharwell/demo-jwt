using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication.Controllers
{
    [Route("jwt")]
    public class JwtController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claims = new List<Claim>()
            {
                new Claim("name", "brian")
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signingCredentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return Ok(new
            {
                access_token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken)
            });
        }

        //[HttpGet("")]
        //public IActionResult Index()
        //{
        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey,
        //        SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

        //    var claims = new List<Claim>()
        //    {
        //        new Claim("name", "brian")
        //    };

        //    var claimsIdentity = new ClaimsIdentity(claims);

        //    var securityTokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Audience = "audience",
        //        Expires = DateTime.Now.AddMinutes(1),
        //        IssuedAt = DateTime.Now,
        //        Issuer = "issuer",
        //        NotBefore = DateTime.Now,
        //        SigningCredentials = signingCredentials,
        //        Subject = claimsIdentity
        //    };

        //    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        //    var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        //    return Ok(new
        //    {
        //        access_token = jwtSecurityTokenHandler.WriteToken(securityToken)
        //    });
        //}

        [HttpPost]
        public IActionResult Validate([FromForm] string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Key));

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = "audience",
                ValidIssuer = "issuer",
                IssuerSigningKey = symmetricSecurityKey,
            };

            var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var jwtSecurityToken);

            return Ok();
        }
    }
}