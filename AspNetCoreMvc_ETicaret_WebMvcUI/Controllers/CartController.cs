using AspNetCoreMvc_ETicaret_Entity.Services;
using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using AspNetCoreMvc_ETicaret_WebMvcUI.SessionExtensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCoreMvc_ETicaret_WebMvcUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartLineService _cartLineService;
        private readonly IAccountService _accountService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartLineService cartLineService, IAccountService accountService, ICartService cartService)
        {
            _productService = productService;
            _cartLineService = cartLineService;
            _accountService = accountService;
            _cartService = cartService;
        }
        List<CartLineViewModel> Cart;


        public IActionResult Index()
        {
            Cart = GetCart();
            ViewBag.total = _cartLineService.TotalPrince(Cart);
            return View(Cart);
        }
        public async Task AddCart(int id, int quantity)
        {
            Cart = GetCart();
            Cart = await _cartLineService.AddCart(Cart, id, quantity);
            SaveCart(Cart,quantity,id);
        }
        public List<CartLineViewModel> GetCart()
        {
            var cart = HttpContext.Session.GetJson<List<CartLineViewModel>>("cart") ?? new List<CartLineViewModel>();
            if (User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var carts = _cartService.GetUserCart(userId);
                if (carts != null)
                {
                    cart = _cartLineService.GetCartLines(carts.Id);
                }
                
            }

            return cart;
        }
        public IActionResult DeleteCart(int id)
        {
            Cart = GetCart();
            Cart = _cartLineService.RemoveCart(Cart, id);
            SaveCart(Cart,0,0);
            return RedirectToAction("Index");
        }
        public async Task SaveCart(List<CartLineViewModel> cartline, int quantity,int productId)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var carts = _cartService.GetUserCart(userId);
            if (User.Identity.IsAuthenticated)
            {
                if (carts == null)
                {
                    var cartid= await _cartService.CreateCart(userId, _cartLineService.TotalPrince(cartline));
                    _cartLineService.CreateCartLine(Cart, cartid, quantity, productId);
                }
                else
                {
                     _cartLineService.CreateCartLine(Cart, carts.Id,quantity,productId);
                }
            }
            else
            {
                HttpContext.Session.SetJson("cart", cartline);
            }
        }
        public IActionResult DropdownRefresh()
        {
            Cart = GetCart();
            ViewBag.total = _cartLineService.TotalPrince(Cart);
            return ViewComponent("CartDropdown", Cart);
        }
        public IActionResult CartCount()
        {
            int count = GetCart().Count();
            return ViewComponent("CartCount", count);
        }
    }
}
