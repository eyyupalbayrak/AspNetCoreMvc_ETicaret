using AspNetCoreMvc_ETicaret_Entity.Entities;
using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvc_ETicaret_Entity.Services
{
    public interface IProductService
    {
        Task Add(ProductViewModel model);
        public void Update(ProductViewModel model);
        public void Delete(ProductViewModel model);
       
        Task<ProductViewModel> GetById(int id);
        Task<IEnumerable<ProductViewModel>> GetListAllByFilter(Expression<Func<Products, bool>> filter, params Expression<Func<Products, object>>[] includes);
        Task<List<ProductViewModel>> GetProductsBySpecs(List<ProductViewModel> model, int? id, string[]? value);
    }
}
