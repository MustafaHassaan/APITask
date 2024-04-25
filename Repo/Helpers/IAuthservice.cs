using Core.Models;
using Microsoft.AspNetCore.Http;
using Repo.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Helpers
{
    public interface IAuthservice
    {
        Task<Authentication> Registerasync(Register Reg);
        Task<Authentication> Loginasync(Login Log);
        Task<string> AddRoleAsync(AddRoles ARole);
    }
}
