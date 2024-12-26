using bookstoresolution.Models;
using bookstoresolution.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace bookstoresolution.Controllers
{
    public class BillingController : Controller
    {

        private readonly BookstoresolutionContext _context;

        public BillingController(BookstoresolutionContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var billings = _context.Billings.Include(b => b.Customer).ToListAsync();
            return View(await billings);
        }

        // GET: Billing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.BillDetails) // Include associated details
                .Include(b => b.Customer) // Include customer details
                .FirstOrDefaultAsync(m => m.BillId == id);

            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }


        // GET: Billing/Create
        public IActionResult Create()
        {
            var model = new BillingCreateViewModel
            {
                Customers = _context.Customers.ToList(),
                Books = _context.Books.Include(b => b.Category).ToList()
            };

            if (model.Customers == null || model.Books == null)
            {
                // Optionally: handle this by redirecting to an error page or showing a message
                return RedirectToAction("Error");  // Example error handling
            }

            return View(model);
        }






        // POST: Billing/Checkout
        [HttpPost]
        public IActionResult Checkout(int customerId, int[] bookIds, IFormCollection form)
        {
            if (bookIds.Length == 0 || customerId <= 0)
            {
                return BadRequest("Invalid input");
            }

            // Ensure that at least one book has a quantity greater than 0
            bool hasValidQuantity = false;

            // Loop through selected books to check quantities
            foreach (var bookId in bookIds)
            {
                // Get the quantity for this specific book from the form data
                var quantityString = form["quantities_" + bookId];

                // Only check the quantity if it has been provided
                if (!string.IsNullOrEmpty(quantityString))
                {
                    if (int.TryParse(quantityString, out int quantity) && quantity > 0)
                    {
                        hasValidQuantity = true;
                        break; // Exit the loop if a valid quantity is found
                    }
                }
            }

            if (!hasValidQuantity)
            {
                return BadRequest("At least one book must have a quantity greater than 0.");
            }

            // Create a new Bill
            var billing = new Billing
            {
                CustomerId = customerId,
                BillDate = DateTime.Now,
                TotalAmount = 0 // Will calculate below
            };

            _context.Billings.Add(billing);
            _context.SaveChanges();

            // Add BillDetails and update book stock
            foreach (var bookId in bookIds)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
                if (book != null)
                {
                    // Get the quantity for this specific book from the form data
                    var quantityString = form["quantities_" + book.BookId];

                    if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity) && quantity > 0 && quantity <= book.Stock)
                    {
                        var totalPrice = book.Price * quantity;

                        // Add BillDetails
                        var billDetail = new BillDetail
                        {
                            BillId = billing.BillId,
                            BookId = book.BookId,
                            Quantity = quantity,
                            TotalPrice = totalPrice
                        };

                        _context.BillDetails.Add(billDetail);

                        // Update Book Stock
                        book.Stock -= quantity;

                        // Update Total Amount
                        billing.TotalAmount += totalPrice;
                    }
                    else if (!string.IsNullOrEmpty(quantityString))
                    {
                        return BadRequest($"Invalid quantity for book {book.Title}.");
                    }
                }
                else
                {
                    return BadRequest("Invalid book or insufficient stock");
                }
            }

            _context.SaveChanges();

            // Redirect to Print page
            return RedirectToAction("Print", new { id = billing.BillId });
        }





        // GET: Billing/Print
        public IActionResult Print(int id)
        {
            var bill = _context.Billings
                .Include(b => b.Customer)
                .Include(b => b.BillDetails)
                .ThenInclude(bd => bd.Book)
                .FirstOrDefault(b => b.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // GET: Billing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.Customer) // Assuming Billing has a navigation property to Customer
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }


        // POST: Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billing = await _context.Billings
                .Include(b => b.BillDetails) // Assuming Billing has a navigation property to BillingDetails
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (billing != null)
            {
                // Remove associated BillingDetails
                _context.BillDetails.RemoveRange(billing.BillDetails);

                // Remove the Billing record
                _context.Billings.Remove(billing);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

       
    }
}