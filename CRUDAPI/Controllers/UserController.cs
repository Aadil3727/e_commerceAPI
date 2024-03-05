using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Models;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;

        public UserController(IUser user)   
        {
            this.user = user;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var data = await user.List();
            if (data == null)
            {
                return NotFound();

            }
            else
            {
                return Ok(data);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var userDto = await user.GetUserById(id);
            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserDTO userDb)
        {
            var result = await user.Create(userDb);
            if (result == "Success")
            {
                return Ok(new { message = "User registered successfully" });
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO userDb)
        {
            var res = await user.Login(userDb);
            if (res == "Success")
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
            var response = user.DeleteUser(id);
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
        public async Task<IActionResult> Edit(EditDTO edit_Dto)
        {

            var response = await user.Edit(edit_Dto);
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
