using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvc_ETicaret_Entity.Services
{
    public interface ICartService
    {
        CartViewModel GetUserCart(int id);
        Task<int> CreateCart(int id,decimal totalprice);
    }
}
