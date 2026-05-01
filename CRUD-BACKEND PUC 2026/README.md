# 🛠️ CrudApp — Backend C# com Entity Framework Core

Aplicação backend em **ASP.NET Core 8** com CRUD completo, Entity Framework Core e SQL Server.
Inspirada no projeto [mf-dev-backend-2026](https://github.com/augusta-bar/mf-dev-backend-2026).

---

## 📁 Estrutura do Projeto

```
CrudApp/
├── Controllers/
│   ├── ProdutosController.cs       ← CRUD MVC (Views HTML)
│   ├── ProdutosApiController.cs    ← CRUD REST API (JSON)
│   └── CategoriasApiController.cs  ← CRUD REST API (JSON)
├── Models/
│   ├── Produto.cs                  ← Entidade Produto
│   ├── Categoria.cs                ← Entidade Categoria (1:N com Produto)
│   └── AppDbContext.cs             ← Contexto do banco (EF Core)
├── Migrations/
│   └── 20260430000001_M01CriacaoInicial.cs
├── appsettings.json                ← Connection string
├── Program.cs                      ← Configuração e pipeline HTTP
└── CrudApp.csproj                  ← Dependências NuGet
```

---

## ⚙️ Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| .NET SDK | 8.0 |
| SQL Server Express | 2019+ |
| Visual Studio | 2022 (ou VS Code) |

---

## 🚀 Como rodar

### 1. Clone o repositório
```bash
git clone <url-do-repo>
cd CrudApp
```

### 2. Configure a connection string

Edite o arquivo `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CrudAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
> Ajuste `Server=` conforme sua instância do SQL Server.

### 3. Restaure os pacotes
```bash
dotnet restore
```

### 4. Execute as migrations (cria o banco e as tabelas)
```bash
dotnet ef database update
```

### 5. Rode a aplicação
```bash
dotnet run
```

---

## 🌐 Endpoints disponíveis

### Interface MVC (HTML)
| URL | Descrição |
|---|---|
| `GET /Produtos` | Lista todos os produtos |
| `GET /Produtos/Create` | Formulário de cadastro |
| `GET /Produtos/Edit/{id}` | Formulário de edição |
| `GET /Produtos/Details/{id}` | Detalhes do produto |
| `GET /Produtos/Delete/{id}` | Confirmação de exclusão |

### REST API (JSON)
| Método | URL | Descrição |
|---|---|---|
| `GET` | `/api/produtosapi` | Listar produtos |
| `GET` | `/api/produtosapi/{id}` | Buscar por ID |
| `POST` | `/api/produtosapi` | Criar produto |
| `PUT` | `/api/produtosapi/{id}` | Atualizar produto |
| `PATCH` | `/api/produtosapi/{id}/preco` | Atualizar só o preço |
| `DELETE` | `/api/produtosapi/{id}` | Excluir produto |
| `GET` | `/api/categoriasapi` | Listar categorias |
| `POST` | `/api/categoriasapi` | Criar categoria |
| `PUT` | `/api/categoriasapi/{id}` | Atualizar categoria |
| `DELETE` | `/api/categoriasapi/{id}` | Excluir categoria |

### Swagger
Acesse `https://localhost:<porta>/swagger` para a documentação interativa.

---

## 📦 Exemplo de payload (POST /api/produtosapi)

```json
{
  "nome": "Notebook Dell",
  "descricao": "Intel Core i7, 16GB RAM, 512GB SSD",
  "preco": 4599.90,
  "quantidade": 10,
  "categoriaId": 1
}
```

---

## 🔄 Comandos úteis do EF Core

```bash
# Criar uma nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar no banco
dotnet ef database update

# Reverter para uma migration específica
dotnet ef database update NomeDaMigrationAnterior

# Remover a última migration (sem aplicar)
dotnet ef migrations remove

# Ver o SQL que seria executado
dotnet ef migrations script
```

---

## 🧩 Conceitos utilizados

| Conceito | Onde está |
|---|---|
| `[Table]` | Mapeia classe → tabela no banco |
| `[Key]` | Define a chave primária |
| `[Required]` | Validação de campo obrigatório |
| `[ForeignKey]` | Define a chave estrangeira |
| `DbContext` | `AppDbContext.cs` |
| `DbSet<T>` | Representa uma tabela |
| `Include()` | Eager loading (JOIN) |
| `SaveChangesAsync()` | Persiste as alterações |
| `ModelState.IsValid` | Valida o model antes de salvar |
| `[ValidateAntiForgeryToken]` | Proteção contra CSRF |
| `TempData` | Mensagem flash entre redirects |
