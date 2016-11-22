using System;

namespace DataAccessLayer
{
    /// <summary>
    /// Model for keeping an audit of all transactions
    /// </summary>
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public virtual PaymentAccount PaymentAccount { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public int Ammount { get; set; }
        public virtual Campaign Campaign { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}