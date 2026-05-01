using System.ComponentModel.DataAnnotations;

namespace ProductsApi.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória.")]
    [MaxLength(50, ErrorMessage = "Categoria deve ter no máximo 50 caracteres.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantidade em estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantidade em estoque não pode ser negativa.")]
    public int StockQuantity { get; set; }
}

public class UpdateProductDto
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória.")]
    [MaxLength(50, ErrorMessage = "Categoria deve ter no máximo 50 caracteres.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantidade em estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantidade em estoque não pode ser negativa.")]
    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
