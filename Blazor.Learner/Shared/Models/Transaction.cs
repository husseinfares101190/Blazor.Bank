using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Learner.Shared.Models
{
   public class Transaction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double TransactionAmount { get; set; } = 0.0;
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public int AccountId { get; set; }

        public bool IsFuture { get; set; } = false;

        public ICollection<BalanceTransaction> BalanceTransactions { get; set; }

        public Transaction()
        {
            BalanceTransactions = new List<BalanceTransaction>();
        }
    }
}
