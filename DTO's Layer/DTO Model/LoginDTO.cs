using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Please Enter Name")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

    }
}
