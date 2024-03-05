using BusinessLayer.Interfaces;
using DTO_s_Layer.DTO_Model;
using DTO_s_Layer.DTO_Model.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory category;

        public CategoryController(ICategory category)
        {
            this.category = category;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var data = await category.List();
            if (data == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(data);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category_DTO cat_db)
        {
            var result = await category.Create(cat_db);
            if (result == "Success")
            {
                return Ok(new { message = "Category Created Successfully" });
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCatDTO edit_Dto)
        {

            var response = await category.Edit(edit_Dto);
            if (response == "Success")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var response = category.DeleteCat(id);
            if (response == "Success")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
