using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Learner.Shared.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Local Account Number mandatory.")]
        public string LocalAccountNumber { get; set; }
        [Required(ErrorMessage = "IBAN mandatory.")]
        [RegularExpression(@"^BE\d{2}\s?([0-9a-zA-Z]{4}\s?){4}[0-9a-zA-Z]{2}$",ErrorMessage ="invaild IBAN")]
        public string IBAN { get; set; }
        public string BankName { get; set; }

        public decimal Currency { get; set; }

        public decimal CurrentBalance { get; set; }

        public ICollection<Balance> Balances { get; set; }
        

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   Id == account.Id &&
                   LocalAccountNumber == account.LocalAccountNumber &&
                   IBAN == account.IBAN &&
                   BankName == account.BankName &&
                   Currency == account.Currency &&
                   CurrentBalance == account.CurrentBalance &&
                   EqualityComparer<ICollection<Balance>>.Default.Equals(Balances, account.Balances);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, LocalAccountNumber, IBAN, BankName, Currency, CurrentBalance, Balances);
        }
    }
}
