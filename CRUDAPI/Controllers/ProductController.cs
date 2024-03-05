using BusinessLayer.Interfaces;
using BusinessLayer.Services.Product;
using DataLayer.Models;
using DTO_s_Layer.DTO_Model.Category;
using DTO_s_Layer.DTO_Model.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct product;

        public ProductController(IProduct product)
        {
            this.product = product;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product_DTO productDTO)
        {
            var result = await product.ProductCreate(productDTO);

            if (result == "Success")
            {
                return Ok(new { message = "Product Created Successfully" });
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var data = await product.List();
            if (data == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(data);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var response = product.DeletePro(id);
            if (response == "Success")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditDTO edit_Dto)
        {

            var response = await product.Edit(edit_Dto);
            if (response == "Success")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var userDto = await product.GetProductById(id);
            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }
    }
}
