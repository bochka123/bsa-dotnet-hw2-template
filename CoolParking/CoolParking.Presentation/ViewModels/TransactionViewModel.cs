using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolParking.Presentation.ViewModels
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public decimal Sum { get; set; }
        public TransactionViewModel(string Id, decimal Sum)
        {
            this.Id = Id;
            this.Sum = Sum;
        }
    }
}
