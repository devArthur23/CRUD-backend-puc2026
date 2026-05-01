using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/categoriasapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        // GET /api/categoriasapi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });

            return categoria;
        }

        // POST /api/categoriasapi
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        // PUT /api/categoriasapi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest(new { mensagem = "ID não confere." });

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categorias.Any(c => c.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE /api/categoriasapi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });

            if (categoria.Produtos.Any())
                return BadRequest(new { mensagem = "Não é possível excluir uma categoria que possui produtos vinculados." });

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Categoria '{categoria.Nome}' excluída com sucesso." });
        }
    }
}
