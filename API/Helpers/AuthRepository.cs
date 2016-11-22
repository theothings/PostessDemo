using API.Models;
using DataAccessLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Helpers
{
    /// <summary>
    /// Provides methods for registering and finding users
    /// </summary>
    public class AuthRepository : IDisposable
    {
        private PostessDB ctx;
        private UserManager<ApplicationUser> userManager;

        public AuthRepository()
        {
            this.ctx = new PostessDB();
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ctx));
        }

        public async Task<IdentityResult> RegisterUser(RegisterBindingModel registerModel)
        {
            Account account = new Account();

            ApplicationUser user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                DefaultAccount = account
            };

            var result = await this.userManager.CreateAsync(user, registerModel.Password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string email, string password)
        {
            ApplicationUser user = await this.userManager.FindAsync(email, password);

            return user;
        }

        public void Dispose()
        {
            this.ctx.Dispose();
            this.userManager.Dispose();
        }
    }
}