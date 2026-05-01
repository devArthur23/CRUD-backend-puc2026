using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Controllers
{
    /// <summary>
    /// Controller MVC responsável pelo CRUD de Produtos.
    /// Cada action retorna uma View (HTML) para o usuário.
    /// </summary>
    public class ProdutosController : Controller
    {
        // Injeção de dependência do contexto do banco
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // ── READ (Listagem) ───────────────────────────────────────────────────

        /// <summary>
        /// GET /Produtos
        /// Lista todos os produtos com a categoria relacionada.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Include() faz o JOIN com a tabela categorias (eager loading)
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nome)
                .ToListAsync();

            return View(produtos);
        }

        // ── READ (Detalhes) ───────────────────────────────────────────────────

        /// <summary>
        /// GET /Produtos/Details/5
        /// Exibe os detalhes de um produto específico.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

        // ── CREATE ────────────────────────────────────────────────────────────

        /// <summary>
        /// GET /Produtos/Create
        /// Exibe o formulário de cadastro.
        /// </summary>
        public IActionResult Create()
        {
            // Popula o SelectList de categorias para o dropdown
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        /// <summary>
        /// POST /Produtos/Create
        /// Recebe os dados do formulário e persiste no banco.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Proteção contra CSRF
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.DataCadastro = DateTime.Now;
                _context.Add(produto);
                await _context.SaveChangesAsync();

                TempData["Mensagem"] = "Produto cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            // Se inválido, recarrega o dropdown e retorna o formulário com erros
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // ── UPDATE ────────────────────────────────────────────────────────────

        /// <summary>
        /// GET /Produtos/Edit/5
        /// Exibe o formulário preenchido com os dados atuais do produto.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        /// <summary>
        /// POST /Produtos/Edit/5
        /// Recebe os dados alterados e atualiza o registro no banco.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto)
        {
            if (id != produto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                    TempData["Mensagem"] = "Produto atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExiste(produto.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // ── DELETE ────────────────────────────────────────────────────────────

        /// <summary>
        /// GET /Produtos/Delete/5
        /// Exibe a tela de confirmação de exclusão.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

        /// <summary>
        /// POST /Produtos/Delete/5
        /// Confirma e executa a exclusão do produto.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Produto excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private bool ProdutoExiste(int id) =>
            _context.Produtos.Any(p => p.Id == id);
    }
}
