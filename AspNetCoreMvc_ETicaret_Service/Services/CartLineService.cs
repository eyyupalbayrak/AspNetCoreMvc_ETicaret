using AspNetCoreMvc_ETicaret_Entity.Entities;
using AspNetCoreMvc_ETicaret_Entity.Services;
using AspNetCoreMvc_ETicaret_Entity.UnitOfWorks;
using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvc_ETicaret_Service.Services
{
    public class CartLineService : ICartLineService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorks _uow;

        public CartLineService(IProductService productService, IMapper mapper, IUnitOfWorks uow)
        {
            _productService = productService;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<List<CartLineViewModel>> AddCart(List<CartLineViewModel> cartline, int id, int quantity)
        {
            var product = await _productService.GetById(id);
            CartLineViewModel newcartLine = new CartLineViewModel();
            newcartLine.Quantity = quantity;
            newcartLine.Product = _mapper.Map<Products>(product);
            newcartLine.ProductId = product.Id;
            newcartLine.TotalPrince = TotalPrince(cartline);
            newcartLine.Price = product.Price;

            if (cartline.Any(cl => cl.ProductId == newcartLine.ProductId))
            {
                foreach (CartLineViewModel item in cartline)
                {
                    if (item.ProductId == newcartLine.ProductId)
                    {
                        item.Quantity += newcartLine.Quantity;
                    }
                }
            }
            else
            {
                cartline.Add(newcartLine);
            }
            return cartline;

        }

        public void CreateCartLine(List<CartLineViewModel> model, int cartid, int quantity,int productId)
        {
            var cartlines = _uow.GetRepository<CartLine>().GetAllNotAsync(x => x.CartId == cartid);

            foreach (var item in model)
            {
                if (cartlines.Any(x => x.ProductId == item.ProductId))
                {
                    foreach (var cart in cartlines)
                    {
                        if (item.ProductId == cart.ProductId &&cart.ProductId == productId)
                        {
                            cart.Quantity += quantity;
                            cart.TotalPrince += item.Quantity * item.Price;
                            cart.Price = item.Price;
                            _uow.GetRepository<CartLine>().Update(cart);
                        }
                    }
                }
                else
                {
                    CartLine cartline = new CartLine()
                    {
                        ProductId = item.ProductId,
                        Quantity = quantity,
                        Price = item.Price,
                        CartId = cartid,
                        TotalPrince = this.TotalPrince(model)
                    };
                    _uow.GetRepository<CartLine>().AddNotAsync(cartline);
                }
            }
            _uow.Commit();
        }

        public List<CartLineViewModel> GetCartLines(int id)
        {
            var cartlines = _uow.GetRepository<CartLine>().GetAllNotAsync(x => x.CartId == id,null,x=>x.Product);
            return _mapper.Map<List<CartLineViewModel>>(cartlines);
        }

        public List<CartLineViewModel> RemoveCart(List<CartLineViewModel> cartline, int id)
        {
            cartline.RemoveAll(cl => cl.ProductId == id);
            return cartline;
        }
        public decimal TotalPrince(List<CartLineViewModel> cartline)
        {
            decimal total = 0;
            if (cartline.Count != null)
            {
                foreach (var item in cartline)
                {
                    total += item.Quantity * item.Price;
                }
            }

            return total;
        }
    }
}
