using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Products;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
   
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _storeDbContext;

        public BuggyController(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var Poduct = _storeDbContext.Products.Find(100);

            if(Poduct is null)
                return NotFound(new ApiResponse(404));

            return Ok(Poduct);
        }

        [HttpGet("badrequests")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("unauthorizes")]
        public ActionResult Getunauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
