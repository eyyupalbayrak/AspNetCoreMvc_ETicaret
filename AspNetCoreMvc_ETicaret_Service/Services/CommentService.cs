using AspNetCoreMvc_ETicaret_Entity.Services;
using AspNetCoreMvc_ETicaret_Entity.UnitOfWorks;
using AspNetCoreMvc_ETicaret_Entity.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wissen.Bright.BlogProject.App.Entity.Entities;

namespace AspNetCoreMvc_ETicaret_Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task Add(CommentViewModel model)
        {
            Comment comment = new Comment();
            comment = _mapper.Map<Comment>(model);
            await _unitOfWorks.GetRepository<Comment>().Add(comment);
            await _unitOfWorks.CommitAsync();
        }

        public async Task<List<CommentViewModel>> GetAllByProductId(int Id)
        {
          var list = await _unitOfWorks.GetRepository<Comment>().GetAll(c => c.ProductId==Id);
            return _mapper.Map<List<CommentViewModel>>(list);
        }
    }
}
