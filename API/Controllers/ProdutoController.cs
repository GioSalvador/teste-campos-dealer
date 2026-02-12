using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using TesteCamposDealer.DB;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/produto")]
    public class ProdutoController : ApiController
    {
        /// <summary>
        /// Recupera o Produto por Id..
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idProduto:int}")]
        public IHttpActionResult GetById(int idProduto)
        {
            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var produto = db.Produtos
                    .FirstOrDefault(p => p.idProduto == idProduto);

                if (produto == null)
                    return Content(HttpStatusCode.NotFound, "Produto não encontrado");

                return Ok(produto);
            }
        }

        /// <summary>
        /// Recupera todos os Produtos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var conn = ConfigurationManager
             .ConnectionStrings["TesteCamposDealerConnectionString"]
             .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;
                var produtos = db.Produtos.ToList();
                return Ok(produtos);
            }
        }

        /// <summary>
        /// Cadastra um Produto
        /// </summary>
        /// <param name="produtoDTO"></param>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ProdutoCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Body não pode ser vazio");

            if (string.IsNullOrWhiteSpace(dto.dscProduto))
                return BadRequest("Descrição é obrigatória");

            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                try
                {
                    var produto = new Produto
                    {
                        dscProduto = dto.dscProduto,
                        precoAtual = dto.preco
                    };

                    db.Produtos.InsertOnSubmit(produto);
                    db.SubmitChanges();

                    var historico = new ProdutoPrecoHistorico
                    {
                        idProduto = produto.idProduto,
                        preco = dto.preco,
                        dataAlteracao = DateTime.Now
                    };

                    db.ProdutoPrecoHistoricos.InsertOnSubmit(historico);

                    db.SubmitChanges();

                    return Content(HttpStatusCode.Created, produto);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        /// <summary>
        /// Altera um Produto pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        [Route("{idProduto:int}")]
        public IHttpActionResult Put(int idProduto, [FromBody] ProdutoCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Body não pode ser vazio");

            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var produto = db.Produtos
                    .FirstOrDefault(p => p.idProduto == idProduto);

                if (produto == null)
                    return Content(HttpStatusCode.NotFound, "Produto não encontrado");

                if (produto.precoAtual != dto.preco)
                {
                    produto.precoAtual = dto.preco;

                    var historico = new ProdutoPrecoHistorico
                    {
                        idProduto = produto.idProduto,
                        preco = dto.preco,
                        dataAlteracao = DateTime.Now
                    };

                    db.ProdutoPrecoHistoricos.InsertOnSubmit(historico);
                }

                produto.dscProduto = dto.dscProduto;

                db.SubmitChanges();

                return Ok("Produto atualizado com sucesso");
            }
        }


        /// <summary>
        /// Deleta um Produto pelo seu id
        /// </summary>
        /// <param name="idProduto"></param>
        [HttpDelete]
        [Route("{idProduto:int}")]
        public IHttpActionResult Delete(int idProduto)
        {
            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var produto = db.Produtos
                    .FirstOrDefault(p => p.idProduto == idProduto);

                if (produto == null)
                    return Content(HttpStatusCode.NotFound, "Produto não encontrado");

                db.Produtos.DeleteOnSubmit(produto);
                db.SubmitChanges();

                return Ok("Produto removido com sucesso");
            }
        }
    }
}