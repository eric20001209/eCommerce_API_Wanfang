using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Models;
using System.Security.Cryptography;
using System.Text;

namespace eCommerce_API.Controllers
{
    [Authorize]
    [Route("api/userRegister")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        rst374_cloud12Context _context = new rst374_cloud12Context();

        [AllowAnonymous]
        [HttpPost("MD5")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Card newCard = new Card();

            //check email exists or not!
            var email = newUser.email;
            bool hasemail = _context.Card.Any(e => e.Email == email);
            var errorMsg = new { error = "Sorry, this email exists already!!!" };
            if (hasemail)
                return BadRequest(errorMsg.error);
            var password = newUser.password;
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string md5password = sBuilder.ToString().ToUpper();
            newCard.Name = newUser.name;
            newCard.Email = newUser.email;
            newCard.Password = md5password; //newUser.password;
            newCard.Type = 6;// newUser.type;
            newCard.AccessLevel = 10;// newUser.accesslevel;

            await _context.Card.AddAsync(newCard);
            await _context.SaveChangesAsync();
            return Ok(
                new { newCard.Name, newCard.Email, newCard.Password, newCard.Type, newCard.AccessLevel }
                );
        }
    }
}