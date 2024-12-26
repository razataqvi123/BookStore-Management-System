using bookstoresolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookstoresolution.Controllers
{

        public class BillingDetailsController : Controller
        {
            private readonly BookstoresolutionContext _context;

            public BillingDetailsController(BookstoresolutionContext context)
            {
                _context = context;
            }

        // GET: BillingDetails/Index/5 (view BillDetails by BillId)
        public async Task<IActionResult> Index()
        {
            // Include related data for display
            var billDetails = _context.BillDetails
                .Include(bd => bd.Book)
                .Include(bd => bd.Bill)
                .ThenInclude(b => b.Customer); // Include customer data if needed

            return View(await billDetails.ToListAsync());
        }

    }
}
