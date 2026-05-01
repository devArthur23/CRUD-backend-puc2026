using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Controllers
{
    /// <summary>
    /// Controller de API REST para Produtos.
    /// Endpoints consumíveis por qualquer client (mobile, frontend, Postman, etc.)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosApiController(AppDbContext context)
        {
            _context = context;
        }

        // ── GET /api/produtosapi ──────────────────────────────────────────────

        /// <summary>
        /// Retorna a lista de todos os produtos com suas categorias.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        // ── GET /api/produtosapi/{id} ─────────────────────────────────────────

        /// <summary>
        /// Retorna um produto específico pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });

            return produto;
        }

        // ── POST /api/produtosapi ─────────────────────────────────────────────

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            produto.DataCadastro = DateTime.Now;
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            // Retorna 201 Created com o produto e a URL de acesso
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // ── PUT /api/produtosapi/{id} ─────────────────────────────────────────

        /// <summary>
        /// Atualiza um produto existente (substituição completa).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest(new { mensagem = "O ID da URL não confere com o ID do body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExiste(id))
                    return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
                throw;
            }

            return NoContent(); // 204 – sucesso sem corpo de resposta
        }

        // ── PATCH /api/produtosapi/{id}/preco ─────────────────────────────────

        /// <summary>
        /// Atualiza apenas o preço de um produto (atualização parcial).
        /// </summary>
        [HttpPatch("{id}/preco")]
        public async Task<IActionResult> PatchPreco(int id, [FromBody] decimal novoPreco)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });

            if (novoPreco <= 0)
                return BadRequest(new { mensagem = "O preço deve ser maior que zero." });

            produto.Preco = novoPreco;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Preço atualizado com sucesso.", produto });
        }

        // ── DELETE /api/produtosapi/{id} ──────────────────────────────────────

        /// <summary>
        /// Exclui um produto pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Produto '{produto.Nome}' excluído com sucesso." });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private bool ProdutoExiste(int id) =>
            _context.Produtos.Any(p => p.Id == id);
    }
}
