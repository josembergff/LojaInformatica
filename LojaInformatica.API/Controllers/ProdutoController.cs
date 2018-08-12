using LojaInformatica.API.Dados;
using LojaInformatica.API.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace LojaInformatica.API.Controllers {
    [Route ("api/produtos")]
    public class ProdutoController : Controller, IEntidadeApi<Produto> {
        private readonly IRepositorio _repositorio;
        public ProdutoController (IRepositorio repositorio) {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult Get () {
            var produtos = _repositorio.Produtos;
            return Ok (produtos);
        }

        [HttpGet ("{id}", Name = "ConsultarProduto")]
        public IActionResult Get (int id) {
            var produto = _repositorio.Produtos
                .PorId (id);

            if (produto == null)
                return NotFound ();

            return Ok (produto);
        }

        [HttpPost]
        public IActionResult Post ([FromBody] Produto produto) {
            if (!produto.EstaValidoParaInsercao ()) {
                return BadRequest (produto.Notifications);
            }

            _repositorio.Acrescentar (produto);

            return CreatedAtRoute ("ConsultarProduto", new { id = produto.Id }, produto);
        }

        [HttpPut]
        public IActionResult Put ([FromBody] Produto produto) {
            if (!produto.EstaValidoParaAtualizacao ()) {
                return BadRequest (produto.Notifications);
            }

            if (!_repositorio.Produtos.ConstaNoBanco (produto.Id))
                return NotFound ();

            _repositorio.Atualizar (produto);

            return NoContent ();
        }

        [HttpDelete ("{id}")]
        public IActionResult Delete (int id) {
            var produto = new Produto { Id = id };
            if (!produto.EstaValidoParaInsercao ()) {
                return BadRequest (produto.Notifications);
            }

            if (!_repositorio.Produtos.ConstaNoBanco (id))
                return NotFound ();

            _repositorio.Remover<Produto> (id);

            return NoContent ();
        }
    }
}