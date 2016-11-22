using API.Helpers;
using API.Models;
using DataAccessLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing accounts
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// Contains the methods for registering and finding users
        /// </summary>
        private AuthRepository authRepository = null;

        /// <summary>
        /// Add dependancy injection
        /// </summary>
        public AccountController()
        {
            this.authRepository = new AuthRepository();
        }

        /// <summary>
        /// Registering a new user
        /// </summary>
        /// <param name="user">Register user model with email and password</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterBindingModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Register the new user
            IdentityResult result = await this.authRepository.RegisterUser(user);

            IHttpActionResult errorResult = GetErrorResult(result);

            if(errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.authRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if(result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
