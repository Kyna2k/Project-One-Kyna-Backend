using KynaShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace KynaShop.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiCommnets : Controller
    {
        private readonly KynaShopContext dpHelper;

        public ApiCommnets(KynaShopContext dpHelper)
        {
            this.dpHelper = dpHelper;
        }

        [HttpPost]
        [Route("addComment")]
        public IActionResult addComment(List<Comment> ds)
        {
            var result = -1;
            for (int i = 0; i < ds.Count;i++)
            {
                dpHelper.Comments.Add(ds[i]);
                result = dpHelper.SaveChanges();
                
            }    
            
            return Ok(result);
        }

    }
}
