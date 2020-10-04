using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blazor.Learner.Shared.Models
{
    public class Balance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime BalanceDate { get; set; }

        public int AccountNumber { get; set; }
      
        public double BalanceAmount { get; set; }

        public Account Account { get; set; }
        public ICollection<BalanceTransaction> BalanceTransactions { get; set; }

       
        public override bool Equals(object obj)
        {
            return obj is Balance balance &&
                   Id == balance.Id &&
                   BalanceDate == balance.BalanceDate &&
                   AccountNumber == balance.AccountNumber &&
                   BalanceAmount == balance.BalanceAmount &&
                   EqualityComparer<ICollection<BalanceTransaction>>.Default.Equals(BalanceTransactions, balance.BalanceTransactions);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, BalanceDate, AccountNumber, BalanceAmount, BalanceTransactions);
        }
    }
}
