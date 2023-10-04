using AspNetCoreMvc_ETicaret_Entity.Services;
using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using AspNetCoreMvc_ETicaret_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc_ETicaret_WebMvcUI.Controllers
{
    public class UrunlerController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        private readonly IFilterSpecService _filterSpecService;
        private readonly IProductSpecsService _productSpecsService;
        private readonly IAccountService _accountService;


        public UrunlerController(IProductService productService, IFilterSpecService filterSpecService, ICommentService commentService, IProductSpecsService productSpecsService, IAccountService accountService)
        {
            _productService = productService;
            _commentService = commentService;
            _filterSpecService = filterSpecService;
            _productSpecsService = productSpecsService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(int? id, string[]? value)
        {
            var products = await _productService.GetListAllByFilter(x => x.IsDeleted == false && x.CategoryId == id);

            if (value.Count() > 0)
            {
                var values = await _productService.GetProductsBySpecs(products.ToList(), id, value);
                ViewBag.specs = await _filterSpecService.GetAll(x => x.CategoryId == id);
                return View(values);
            }



            ViewBag.specs = await _filterSpecService.GetAll(x => x.CategoryId == id);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            //ViewBag.Comments = await _commentService.GetAllByProductId(id);
            var model = await _productService.GetById(id);
            ViewBag.specs = await _productSpecsService.GetListAllByFilter(x => x.ProductId == id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(int Id,string Message)
        {
            var user = await _accountService.Find(User.Identity.Name);
            CommentViewModel model = new()
            {
                ProductId = Id,
                Description = Message,
                UserId = user.Id,
            };
            await _commentService.Add(model);
            return RedirectToAction("Index");
        }
        
    }
}
