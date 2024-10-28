using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Respository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly StoreDbContext dbContext;

        public BuggyController(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }
        [HttpGet("Unautherized")]
        public ActionResult GetUnautherized()
        {
            return Unauthorized(new ApiResponse(401));
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product = dbContext.Products.Find(100);
            return Ok(product.ToString());
        }
        [HttpGet("ValidationError/{id}")]
        public ActionResult GetValidationError(int id)
        {
            return Ok(dbContext.Products.Find(id));
        }
    }
}
