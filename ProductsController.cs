using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Controllers;

/// <summary>
/// Endpoints para gerenciamento de Produtos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retorna todos os produtos com filtros opcionais.
    /// </summary>
    /// <param name="category">Filtra por categoria (opcional).</param>
    /// <param name="isActive">Filtra por status ativo/inativo (opcional).</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] bool? isActive)
    {
        var products = await _repository.GetAllAsync(category, isActive);
        var response = products.Select(MapToDto);
        return Ok(response);
    }

    /// <summary>
    /// Retorna um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            return NotFound(new { message = $"Produto com ID {id} não encontrado." });

        return Ok(MapToDto(product));
    }

    /// <summary>
    /// Cria um novo produto.
    /// </summary>
    /// <param name="dto">Dados do produto a ser criado.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = dto.Category,
            StockQuantity = dto.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="id">ID do produto a ser atualizado.</param>
    /// <param name="dto">Dados atualizados do produto.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = dto.Category,
            StockQuantity = dto.StockQuantity,
            IsActive = dto.IsActive
        };

        var updated = await _repository.UpdateAsync(id, product);
        if (updated is null)
            return NotFound(new { message = $"Produto com ID {id} não encontrado." });

        return Ok(MapToDto(updated));
    }

    /// <summary>
    /// Remove um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto a ser removido.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Produto com ID {id} não encontrado." });

        return NoContent();
    }

    private static ProductResponseDto MapToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Category = p.Category,
        StockQuantity = p.StockQuantity,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    };
}
