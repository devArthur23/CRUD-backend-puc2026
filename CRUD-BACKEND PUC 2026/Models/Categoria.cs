using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudApp.Models
{
    /// <summary>
    /// Categoria à qual um produto pertence.
    /// </summary>
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(80)]
        public string Nome { get; set; }

        [MaxLength(200)]
        public string Descricao { get; set; }

        // Propriedade de navegação: uma categoria tem muitos produtos
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
