using ApiAngularApp.Data;
using ApiAngularApp.Models.Domain;
using ApiAngularApp.Models.DTO;
using ApiAngularApp.Respositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ApiAngularApp.Controllers
{
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        

        private readonly IcategoryRepository categoryRepository;

        public CategoriesController(IcategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        //


        [HttpGet]
        public async Task<ActionResult> Getcategory()
        {
            try
            {
                return Ok(await categoryRepository.Getcategory());
            }
            catch (System. Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        //
        [HttpPost]
        public async Task<IActionResult>CreateCategory(CreateCategoryRequestDTO request)
        {
            //Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            await categoryRepository.CreateAsync(category);

                        // Domain model to Dto
            var response = new CategoryDto
            {
                ID = category.ID,
                Name = category.Name,
                UrlHandle = category.UrlHandle

            };

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Category>> Getcategoryid(Guid id)
        {
            try
            {
                var result = await categoryRepository.Getcategoryid(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (System. Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Category>> UpdateCategory(Guid id, Category category)
        {
            try
            {
               // if (id = category.ID)
                    //return BadRequest("Employee ID mismatch");

                var employeeToUpdate = await categoryRepository.Getcategoryid(id);

                if (employeeToUpdate == null)
                    return NotFound($"Employee with Id = {id} not found");

                return await categoryRepository.UpdateCategory(category);
            }
            catch  (System. Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
        

    }
}
