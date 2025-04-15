using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int ProductId, int qty = 1)
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var product = await _context.Products.Where(x => x.Id == ProductId).FirstOrDefaultAsync();

            if (product == null)
            {
                return BadRequest();
            }

            var cart = new Cart { ProductId = ProductId, Qty = qty, UserId = currentuser.Id };

            _context.Add(cart);

            await _context.SaveChangesAsync();

            return View(cart);
        }
    }
}
