using System;
using System.Collections.Generic;

namespace Blazor.Learner.Shared.Models
{
    public class BalanceTransaction
    {
        public int BalanceId { get; set; }
        public Balance Balance { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BalanceTransaction transaction &&
                   EqualityComparer<Transaction>.Default.Equals(Transaction, transaction.Transaction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Transaction);
        }
    }
}
