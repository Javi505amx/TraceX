using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TraceX.Application.DTOs.Products;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;

namespace TraceX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductDto> _createProductDtoValidator;
        private readonly IValidator<UpdateProductDto> _updateProductDtoValidator;


        public ProductsController(IProductRepository productRepository,
                                IValidator<CreateProductDto> createProductDtoValidator,
                                IValidator<UpdateProductDto> updateProductDtoValidator)
        {
            _productRepository = productRepository;
            _createProductDtoValidator = createProductDtoValidator;
            _updateProductDtoValidator = updateProductDtoValidator;

        }

        [HttpGet] // GET: api/products/
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = products.Adapt<List<ProductDto>>();
            return Ok(productDtos);
        }

        [HttpGet("{id:int}")] // GET: api/products/{id}
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var productDto = product.Adapt<ProductDto>();

            return Ok(productDto);
        }

        [HttpPost] // POST: api/products/
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
        {
            // check: this null reference
            if (dto == null) return BadRequest("Product info dto is required");

            var validationResult = await _createProductDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            var existingProduct = await _productRepository.GetByNameAsync(dto.Name);
            if (existingProduct != null)
                return Conflict(new { Message = "There's already a product with this name in database" });

            var productEntity = dto.Adapt<Product>();
            var createdProduct = await _productRepository.AddAsync(productEntity);
            var responseDto = createdProduct.Adapt<ProductDto>();

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = responseDto.Id },
                responseDto);
        }


        [HttpPut("{id:int}")] // PUT: api/products/{id}
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            if (dto == null) return BadRequest("Product info is required");

            var validationResult = await _updateProductDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            dto.Adapt(existingProduct);

            await _productRepository.UpdateAsync(existingProduct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            await _productRepository.DeleteAsync(id);
            return NoContent();
        }


    }
}
