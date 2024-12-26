using bookstoresolution.Models;

namespace bookstoresolution.ViewModels  
{
    public class BillingCreateViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Book> Books { get; set; }
    }
}
