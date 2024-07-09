using DemoRabbitMq.Implement;
using DemoRabbitMqCRUD.Application;
using DemoRabbitMqCRUD.Domain;
using DemoRabbitMqCRUD.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqCRUD.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductProducter _productProducter;
        public ProductController()
        {
            _productProducter = new ProductProducter();
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            _productProducter.CreateProduct(product);
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(Guid id)
        {
            try
            {
                _productProducter.GetProductById(id);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            try
            {
                _productProducter.UpdateProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                _productProducter.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
