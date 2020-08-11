using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Data;
using eCommerce_API.Dto;
using eCommerce_API.Services;
using eCommerce_API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace eCommerce_API.Controllers
{
    [Authorize]
    [Route("api/dealerLogin")]
    [ApiController]
    public class DealerLoginController : ControllerBase
    {
        rst374_cloud12Context _context = new rst374_cloud12Context();
        iMailService _imailService;
        public DealerLoginController(iMailService imailService)
        {
            _imailService = imailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult login([FromBody] LoginDto mylogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var myUserlist = _context.Card
                        .Where(c => c.Type == 2)            //dealer login
                        .Select(c => new
                        {
                            id = c.Id,
                            name = c.Name,
                            login_email = c.Email,
                            password = c.Password,
                            phone = c.Phone,
                            address1 = c.Address1,
                            address2 = c.Address2,
                            address3 = c.Address3,
                            city = c.City,
                            dealer_level = c.DealerLevel,
                            country = c.Country

                        }).OrderBy(c => c.id).ToList();

            //obtain login_email and pw
            string email = mylogin.login_email;
            string passowrd = mylogin.password;
            string name = mylogin.name;

            //encrypt with md5
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(passowrd));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string md5password = sBuilder.ToString().ToUpper();
            foreach (var a in myUserlist)
            {
                if (a.password == md5password && a.login_email == email)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, a.login_email)
                    };
                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["TokenSecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "",
                        audience: "",
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds
                        );

                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            login_email = a.login_email,
                            id = a.id,
                            name = a.name,
                            password = mylogin.password,
                            phone = a.phone,
                            address1 = a.address1,
                            address2 = a.address2,
                            address3 = a.address3,
                            city = a.city,
                            dealer_level = a.dealer_level,
                            country = a.country
                        });
                }
                else
                {
                    continue;
                }
            }

            return BadRequest(
                new { error = "Account not exists!!" }
                );
        }
        [AllowAnonymous]
        [HttpPost("sendPw")]
        public async Task<IActionResult> sendpw([FromBody] SendPasswordDto sendpw)
        {
            if (sendpw == null)
                return BadRequest("No account found!");
            //check if existing user
            var login_email = _context.Card;

            //if true, generate a new pw, update db, send to user
            if (login_email.Any(c => c.Email == sendpw.emailto))
            {
                Card this_card = _context.Card.Where(c => c.Email == sendpw.emailto).FirstOrDefault();
                var card_id = this_card.Id;
                string new_pw = Common.GenerateRandomString(8);
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(new_pw));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                string md5password = sBuilder.ToString().ToUpper();
                this_card.Password = md5password;

                _context.Card.Update(this_card);
                await _context.SaveChangesAsync();
                try
                {
                    _imailService.sendEmail(sendpw.emailto, new_pw);
                    return Ok(new
                    {
                        pw = new_pw,
                        md5password = md5password
                    });
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            else
                return NotFound(); ;
        }
        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="patchMylogin"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPatch("ResetPassword/{userId}")]
        public IActionResult checkpw([FromBody] JsonPatchDocument<LoginDto> patchMylogin, int userId)
        {
            if (patchMylogin == null)
                return BadRequest();

            var accountToPatch = _context.Card.FirstOrDefault(c => c.Id == userId);
            if (accountToPatch == null)
                return NotFound();

            var newAccountToPatch = new LoginDto()
            {
                id = accountToPatch.Id,
                password = accountToPatch.Password
            };

            patchMylogin.ApplyTo(newAccountToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            accountToPatch.Password = newAccountToPatch.password;

            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(accountToPatch.Password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string md5password = sBuilder.ToString().ToUpper();

            accountToPatch.Password = md5password;
            _context.Card.Update(accountToPatch);
            _context.SaveChanges();


            //return NoContent();
            return Ok(new
            {
                password = newAccountToPatch.password,
                md5password = md5password
            });
        }
    }
}