using DataLayer.Models;
using DTO_s_Layer.DTO_Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUser
    {
        Task<string> Create(UserDTO user_Db);
        Task<string> Login(LoginDTO user_Db);

        Task<List<User_db>> List();
        Task<User_db> GetUserById(Guid id);
        Task<bool> UserExists(string Email);
        Task<string> ForgotPassword(string email);
        Task<string> ResetPassword(string token, string newPassword);

        string DeleteUser(Guid id);

        Task<string> Edit(EditDTO edit_Dto);
    }
}
