using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer;
using API.Models;
using Stripe;

namespace API.Controllers
{
    [Authorize]
    public class PaymentAccountsController : BaseApiController
    {
        private IPostessDB db = new PostessDB();

        public PaymentAccountsController() { }

        public PaymentAccountsController(IPostessDB context)
        {
            db = context;
        }

        // GET: api/PaymentAccounts
        public IEnumerable<PaymentAccount> GetPaymentAccount()
        {
            int accountId = this.GetAccountId();
            // Get the payments account for the current user
            return db.Accounts.Where(a => a.Id == accountId).FirstOrDefault().PaymentAccounts.Where(p => p.IsDeleted == false);
        }

        // GET: api/PaymentAccounts/5
        [ResponseType(typeof(PaymentAccount))]
        public IHttpActionResult GetPaymentAccount(int id)
        {
            int accountId = this.GetAccountId();

            PaymentAccount paymentAccount = db.PaymentAccount.Where(p => p.Id == id && p.AccountId == accountId).FirstOrDefault();

            if (paymentAccount == null || paymentAccount.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(paymentAccount);
        }

        // GET: api/DefaultPaymentAccounts
        [ResponseType(typeof(PaymentAccount))]
        public IHttpActionResult GetDefaultPaymentAccount()
        {
            int accountId = this.GetAccountId();
            PaymentAccount paymentAccount = db.Accounts.Where(a => a.Id == accountId).FirstOrDefault().DefaultPaymentAccount;
            if (paymentAccount == null || paymentAccount.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(paymentAccount);
        }
        

        // POST: api/PaymentAccounts
        [ResponseType(typeof(PaymentAccount))]
        public IHttpActionResult PostPaymentAccount(StripeBindingModel stripeBindingModel)
        {
            int accountId = this.GetAccountId();
            Account account = db.Accounts.Find(accountId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use stripe to get the customer token
            string stripeCustomerToken = "";

            var myCustomer = new StripeCustomerCreateOptions();
            myCustomer.SourceToken = stripeBindingModel.CardToken;
            var customerService = new StripeCustomerService();
            var stripeCustomer = customerService.Create(myCustomer);

            PaymentAccount paymentAccount = new PaymentAccount(PaymentMethod.Stripe, stripeCustomerToken);
            paymentAccount.AccountId = accountId;
            
            // updae the default payment account as the new account
            account.DefaultPaymentAccount = paymentAccount;
            db.SetModified(account);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paymentAccount.Id }, paymentAccount);
        }

        // DELETE: api/PaymentAccounts/5
        [ResponseType(typeof(PaymentAccount))]
        public IHttpActionResult DeletePaymentAccount(int id)
        {
            PaymentAccount paymentAccount = db.PaymentAccount.Find(id);
            if (paymentAccount == null || paymentAccount.IsDeleted == true)
            {
                return NotFound();
            }

            paymentAccount.IsDeleted = true;
            db.SetModified(paymentAccount);
            db.SaveChanges();

            return Ok(paymentAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentAccountExists(int id)
        {
            return db.PaymentAccount.Count(e => e.Id == id) > 0;
        }
    }
}