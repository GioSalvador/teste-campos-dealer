using System;
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
        private readonly string conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

        /// <summary>
        /// Recupera todos os produtos
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var produtos = db.Produtos
                    .Select(p => new ProdutoDTO
                    {
                        idProduto = p.idProduto,
                        dscProduto = p.dscProduto,
                        precoAtual = p.precoAtual
                    })
                    .ToList();

                return Ok(produtos);
            }
        }

        /// <summary>
        /// Recupera produto por ID
        /// </summary>
        [HttpGet]
        [Route("{idProduto:int}")]
        public IHttpActionResult GetById(int idProduto)
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var produto = db.Produtos
                    .Where(p => p.idProduto == idProduto)
                    .Select(p => new ProdutoDTO
                    {
                        idProduto = p.idProduto,
                        dscProduto = p.dscProduto,
                        precoAtual = p.precoAtual
                    })
                    .FirstOrDefault();

                if (produto == null)
                    return Content(HttpStatusCode.NotFound,
                        new { message = "Produto não encontrado." });

                return Ok(produto);
            }
        }

        /// <summary>
        /// Cadastra um novo produto
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] ProdutoCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Body da requisição não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.dscProduto))
                return BadRequest("Descrição é obrigatória.");

            if (dto.preco <= 0)
                return BadRequest("Preço deve ser maior que zero.");

            try
            {
                using (var db = new DBTesteCamposDealerDataContext(conn))
                {
                    var produto = new Produto
                    {
                        dscProduto = dto.dscProduto,
                        precoAtual = dto.preco
                    };

                    db.Produtos.InsertOnSubmit(produto);
                    db.SubmitChanges(); // 🔥 PRIMEIRO SALVA O PRODUTO

                    var historico = new ProdutoPrecoHistorico
                    {
                        idProduto = produto.idProduto, // AGORA o ID existe
                        preco = dto.preco,
                        dataAlteracao = DateTime.Now
                    };

                    db.ProdutoPrecoHistoricos.InsertOnSubmit(historico);
                    db.SubmitChanges(); // 🔥 SALVA O HISTÓRICO

                    var produtoDTO = new ProdutoDTO
                    {
                        idProduto = produto.idProduto,
                        dscProduto = produto.dscProduto,
                        precoAtual = produto.precoAtual
                    };

                    return Content(HttpStatusCode.Created, produtoDTO);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// Atualiza produto existente
        /// </summary>
        [HttpPut]
        [Route("{idProduto:int}")]
        public IHttpActionResult Put(int idProduto, [FromBody] ProdutoUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest("Body da requisição não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(dto.dscProduto))
                return BadRequest("Descrição é obrigatória.");

            if (dto.preco <= 0)
                return BadRequest("Preço deve ser maior que zero.");

            try
            {
                using (var db = new DBTesteCamposDealerDataContext(conn))
                {
                    var produto = db.Produtos
                        .FirstOrDefault(p => p.idProduto == idProduto);

                    if (produto == null)
                        return Content(HttpStatusCode.NotFound,
                            new { message = "Produto não encontrado." });

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

                    return Ok(new { message = "Produto atualizado com sucesso." });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Remove produto por ID
        /// </summary>
        [HttpDelete]
        [Route("{idProduto:int}")]
        public IHttpActionResult Delete(int idProduto)
        {
            try
            {
                using (var db = new DBTesteCamposDealerDataContext(conn))
                {
                    var produto = db.Produtos
                        .FirstOrDefault(p => p.idProduto == idProduto);

                    if (produto == null)
                        return Content(HttpStatusCode.NotFound,
                            new { message = "Produto não encontrado." });

                    db.Produtos.DeleteOnSubmit(produto);
                    db.SubmitChanges();

                    return Ok(new { message = "Produto removido com sucesso." });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
