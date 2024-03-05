using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_s_Layer.DTO_Model
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string ProfileImg { get; set; }

        public string Password { get; set; }
    }   
}
