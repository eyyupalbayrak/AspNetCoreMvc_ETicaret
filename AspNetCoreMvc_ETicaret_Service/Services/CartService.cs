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
    public class CartService : ICartService
    {
        private readonly IUnitOfWorks _uow;
        private readonly IMapper _mapper;


        public CartService(IUnitOfWorks uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CreateCart(int id, decimal totalprice)
        {
            Cart cart = new Cart()
            {
                UserId = id,
                TotalPrice = totalprice
            };
            await _uow.GetRepository<Cart>().Add(cart);
            _uow.Commit();
            return cart.Id;
        }

        public CartViewModel GetUserCart(int id)
        {
            var cart = _uow.GetRepository<Cart>().GetNotAsync(x=>x.UserId==id);
            return _mapper.Map<CartViewModel>(cart);
        }
    }
}
