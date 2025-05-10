using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Model;

namespace Shipping.Controllers
{
    [Route("ship/[controller]")]
    [ApiController]
    public class ShippingRequestController : ControllerBase
    {
        private readonly ShippingDbContext _context;

        public ShippingRequestController(ShippingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateRequest([FromBody] ShippingRequest request)
        {
            if (request == null)
            {
                return BadRequest("Please enter details.");
            }

            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return BadRequest("Customer ID is required.");
            }

            if (request.Products != null && request.Products.Count > 0)
            {
                foreach (var product in request.Products)
                {
                    if (product.ShippingWeight <= 0 ||
                        product.ShippingHeight <= 0 ||
                        product.ShippingWidth <= 0 ||
                        product.ShippingDate == default)
                    {
                        return BadRequest("All products must have valid weight, height, width, and date.");
                    }

                    if (product.ShippingMode == "1")
                    {
                        product.ShippingMode = ShippingMode.AIR.ToString();
                    }
                    else if (product.ShippingMode == "2")
                    {
                        product.ShippingMode = ShippingMode.SEA.ToString();
                    }
                    else
                    {
                        return BadRequest("ShippingMode must be '1' (AIR) or '2' (SEA).");
                    }
                }
            }

            request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var request = await _context.Requests.Include(r => r.Products).FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound("Request not found.");
            }

            return Ok(request);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _context.Requests.Include(r => r.Products).ToListAsync();

            return Ok(requests);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] ShippingRequest updatedRequest)
        {
            var existingRequest = await _context.Requests.Include(r => r.Products).FirstOrDefaultAsync(r => r.RequestId == id);

            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            existingRequest.CustomerId = updatedRequest.CustomerId;
            existingRequest.ShippingAddress = updatedRequest.ShippingAddress;
            existingRequest.Country = updatedRequest.Country;
            existingRequest.Status = updatedRequest.Status;

            if (updatedRequest.Products != null && updatedRequest.Products.Count > 0)
            {
                _context.Products.RemoveRange(existingRequest.Products); 
                foreach (var product in updatedRequest.Products)
                {
                    if (product.ShippingMode == "1")
                    {
                        product.ShippingMode = ShippingMode.AIR.ToString();
                    }
                    else if (product.ShippingMode == "2")
                    {
                        product.ShippingMode = ShippingMode.SEA.ToString();
                    }
                    else
                    {
                        return BadRequest("must be '1' (AIR) or '2' (SEA)");
                    }

                    product.RequestId = id;  
                }
                existingRequest.Products = updatedRequest.Products;
            }

            await _context.SaveChangesAsync();

            return Ok(existingRequest);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.Include(r => r.Products).FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound("not found.");
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok("deleted");
        }
    }
}
