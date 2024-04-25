using Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repo.UOW.Unitofworkservices;

namespace APITask.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IUnitofwork _IUW;
        public ProductController(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        [HttpGet("GetallProducts")]
        public async Task<IActionResult> GetallProducts()
        {
            var Pro = _IUW.Products.GetAll();
            return Ok(Pro);
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Products Pro)
        {
            if (Pro == null)
            {
                return BadRequest();
            }
            else
            {
                _IUW.Products.Insert(new Products
                {
                    Name = Pro.Name,
                    Description = Pro.Description,
                    Photopath = Pro.Photopath,
                    Price = Pro.Price
                });
                _IUW.Complete();
                return Ok();
            }
        }

        [HttpPost("GetSingleProduct")]
        public async Task<IActionResult> GetSingleProduct(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                var data = _IUW.Products.Get(id);
                return Ok(data);
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Products Pro)
        {
            if (Pro == null)
            {
                return BadRequest();
            }
            else
            {
                Products UPro = new Products();
                UPro.Id = Pro.Id;
                UPro.Name = Pro.Name;
                UPro.Description = Pro.Description;
                UPro.Photopath = Pro.Photopath;
                UPro.Price = Pro.Price;
                _IUW.Products.Update(UPro);
                _IUW.Complete();
                return Ok();
            }
        }
        [HttpDelete("DelProduct")]
        public IActionResult DelProduct(int id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                _IUW.Products.Delbyid(id);
                _IUW.Complete();
            }
            return Ok();
        }
    }
}
