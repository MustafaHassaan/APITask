using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repo.AuthModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Helpers
{
    public class Authservice : IAuthservice
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public Authservice(UserManager<Users> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Authentication> Registerasync(Register Reg)
        {
            if (await _userManager.FindByEmailAsync(Reg.Email) is not null)
                return new Authentication { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(Reg.Username) is not null)
                return new Authentication { Message = "Username is already registered!" };

            var user = new Users
            {
                UserName = Reg.Username,
                Email = Reg.Email,
                Firstname = Reg.Firstname,
                Lastname = Reg.Lastname
            };

            var result = await _userManager.CreateAsync(user, Reg.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new Authentication { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new Authentication
            {
                Email = user.Email,
                Expireson = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<string> AddRoleAsync(AddRoles ARol)
        {
            var user = await _userManager.FindByIdAsync(ARol.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(ARol.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, ARol.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, ARol.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        public async Task<Authentication> Loginasync(Login log)
        {
            var authModel = new Authentication();

            var user = await _userManager.FindByEmailAsync(log.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, log.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Expireson = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(Users user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
