using AspNetCoreMvc_ETicaret_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wissen.Bright.BlogProject.App.Entity.Entities
{
    public class Comment : BaseEntity
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public virtual Products Products { get; set; }
    }
}
