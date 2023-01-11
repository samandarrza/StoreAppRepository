using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Api.Admin.Dtos;
using Store.Api.Admin.Dtos.ProductDtos;
using Store.Api.Helpers;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using System.Data;

namespace Store.Api.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IMapper mapper, IWebHostEnvironment env, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _env = env;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm]ProductPostDto postDto)
        {
            if (!await _categoryRepository.IsExistAsync(x => x.Id == postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Category not found!" } });

            if (await _productRepository.IsExistAsync(x => x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Product already exist!" } });


            Product product = _mapper.Map<Product>(postDto);
            product.Image = FileManager.Save(postDto.ImageFile, _env.WebRootPath, "uploads/products");

            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();

            return StatusCode(201, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product product =await _productRepository.GetAsync(x => x.Id == id,"Category");
            if (product == null) return NotFound();

            ProductGetDto productDto = _mapper.Map<ProductGetDto>(product);
            return Ok(productDto);
        }
        
        [HttpGet("")]
        public IActionResult GetAll(int page = 1)
        {
            var query = _productRepository.GetAll(x => !x.IsDeleted);

            var productDto = _mapper.Map<List<ProductListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginatedListDto<ProductListItemDto> model = 
                new PaginatedListDto<ProductListItemDto>(productDto, page, 4, query.Count());

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm]ProductPutDto putDto)
        {
            Product product = await _productRepository.GetAsync(x=>x.Id == id);

            if (product == null) return NotFound();

            if (product.CategoryId != putDto.CategoryId && !await _productRepository.IsExistAsync(x => x.Id == putDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Catgory not found!" } });


            if (product.Name != putDto.Name && await _productRepository.IsExistAsync(x => x.Id != id && x.Name == putDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Product already exist!" } });


            product.Name = putDto.Name;
            product.CostPrice = putDto.CostPrice;
            product.SalePrice = putDto.SalePrice;
            product.DiscountPercent = putDto.DiscountPercent;
            product.CategoryId = putDto.CategoryId;

            await _productRepository.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _productRepository.GetAsync(x => x.Id == id);
            if (product == null) return NotFound();

            _productRepository.Remove(product);
            _productRepository.Commit();

            return NoContent();
        }
    }
}
