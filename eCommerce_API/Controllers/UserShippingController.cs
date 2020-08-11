using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eCommerce_API.Dto;
using eCommerce_API.Data;
using eCommerce_API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace eCommerce_API.Controllers
{
    [Route("api/shipping")]
    [ApiController]
    public class UserShippingController : ControllerBase
    {
        private readonly rst374_cloud12Context _context = new rst374_cloud12Context();

        [HttpGet("{userid}")]
        public IActionResult shippingAddressList(int userid)
        {
            var shiptoList = _context.Card.Where(c => c.MainCardId == userid)
                .Select(c => new ShippingToDto
                {
                    id = c.Id,
                    name = c.Name,
                    company = c.Company,
                    address1 = c.Address1,
                    address2 = c.Address2,
                    address3 = c.Address3,
                    city = c.City,
                    country = c.Country,
                    zip = c.Zip,
                    phone = c.Phone,
                    contact = c.Contact,
                    note = c.Note
                }).ToList();
            return Ok(shiptoList);
        }

        [HttpPost("{userid}")]
        public async Task<IActionResult> addShippingAddress(int userid, [FromBody] AddShippingDto newShipping)
        {
            var shippingToAdd = new Card();
            shippingToAdd.MainCardId = userid;
            shippingToAdd.Name = newShipping.name;
            shippingToAdd.Company = newShipping.company;
            shippingToAdd.Address1 = newShipping.address1;
            shippingToAdd.Address2 = newShipping.address2;
            shippingToAdd.Address3 = newShipping.address3;
            shippingToAdd.City = newShipping.city;
            shippingToAdd.Country = newShipping.country;
            shippingToAdd.Phone = newShipping.phone;
            shippingToAdd.Contact = newShipping.contact;
            shippingToAdd.Zip = newShipping.zip;
            shippingToAdd.Note = newShipping.note;
            shippingToAdd.Email =  DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            try
            {
                await _context.AddAsync(shippingToAdd);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
            return Ok();
        }

        [HttpDelete("del/{id}")]
        public async Task<IActionResult> deleteShippingAddress(int id)
        {
            var shippingToDelete = _context.Card.Where(c => c.Id == id).FirstOrDefault();

            if (shippingToDelete == null)
                return NotFound();
            try
            {
                _context.Card.Remove(shippingToDelete);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            return NoContent();
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> updateShippingAddress(int id, [FromBody] JsonPatchDocument<UpdateShippingDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var shippingAddressToUpdate = _context.Card.Where(c => c.Id == id).FirstOrDefault();
            if (shippingAddressToUpdate == null)
                return NotFound();

            var shippingToPatch = new UpdateShippingDto()
            {
                name = shippingAddressToUpdate.Name,
                company = shippingAddressToUpdate.Company,
                address1 = shippingAddressToUpdate.Address1,
                address2 = shippingAddressToUpdate.Address2,
                address3 = shippingAddressToUpdate.Address3,
                city = shippingAddressToUpdate.City,
                country = shippingAddressToUpdate.Country,
                phone = shippingAddressToUpdate.Phone,
                contact = shippingAddressToUpdate.Contact,
                note = shippingAddressToUpdate.Note,
                zip=shippingAddressToUpdate.Zip
            };

            patchDoc.ApplyTo(shippingToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            shippingAddressToUpdate.Name = shippingToPatch.name;
            shippingAddressToUpdate.Company = shippingToPatch.company;
            shippingAddressToUpdate.Address1 = shippingToPatch.address1;
            shippingAddressToUpdate.Address2 = shippingToPatch.address2;
            shippingAddressToUpdate.Address3 = shippingToPatch.address3;
            shippingAddressToUpdate.City = shippingToPatch.city;
            shippingAddressToUpdate.Country = shippingToPatch.country;
            shippingAddressToUpdate.Phone = shippingToPatch.phone;
            shippingAddressToUpdate.Contact = shippingToPatch.contact;
            shippingAddressToUpdate.Note = shippingToPatch.note;
            shippingAddressToUpdate.Zip = shippingToPatch.zip;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}