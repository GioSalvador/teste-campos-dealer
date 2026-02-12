using System;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Web.Http;
using TesteCamposDealer.DB;
using TesteCamposDealer.Models.DTO;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/venda")]
    public class VendaController : ApiController
    {
        /// <summary>
        /// Recupera a Venda por Id..
        /// </summary>
        /// <param name="idVenda"></param>
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
                var vendas = db.Vendas.ToList();
                return Ok(vendas);
            }
        }

        /// <summary>
        /// Recupera todas as vendas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{idVenda:int}")]
        public IHttpActionResult GetById(int idVenda)
        {
            var conn = ConfigurationManager
                 .ConnectionStrings["TesteCamposDealerConnectionString"]
                 .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var venda = db.Vendas
                    .FirstOrDefault(v => v.idVenda == idVenda);

                if (venda == null)
                    return Content(HttpStatusCode.NotFound, "Venda não encontrada");

                return Ok(venda);
            }
        }

        /// <summary>
        /// Recupera todas as vendas de um cliente
        /// </summary>
        [HttpGet]
        [Route("cliente/{idCliente:int}")]
        public IHttpActionResult GetByCliente(int idCliente)
        {
            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var clienteExiste = db.Clientes
                    .Any(c => c.idCliente == idCliente);

                if (!clienteExiste)
                    return Content(HttpStatusCode.NotFound, "Cliente não encontrado");

                var vendas = db.Vendas
                    .Where(v => v.idCliente == idCliente)
                    .ToList();

                return Ok(vendas);
            }
        }

        /// <summary>
        /// Cadastra uma venda
        /// </summary>
        /// <param name="cliente"></param>

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] VendaDTO vendaDTO)
        {
            if (vendaDTO == null || vendaDTO.itens == null || !vendaDTO.itens.Any())
                return BadRequest("Venda deve conter ao menos um item");

            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var cliente = db.Clientes
                    .FirstOrDefault(c => c.idCliente == vendaDTO.idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound, "Cliente não encontrado");

                var venda = new Venda
                {
                    idCliente = vendaDTO.idCliente,
                    dthRegistro = DateTime.Now
                };

                db.Vendas.InsertOnSubmit(venda);

                decimal totalVenda = 0;

                foreach (var itemDTO in vendaDTO.itens)
                {
                    var produto = db.Produtos
                        .FirstOrDefault(p => p.idProduto == itemDTO.idProduto);

                    if (produto == null)
                        return Content(HttpStatusCode.NotFound, $"Produto {itemDTO.idProduto} não encontrado");

                    decimal totalItem = produto.precoAtual * itemDTO.quantidade;

                    var vendaItem = new VendaItem
                    {
                        Venda = venda,
                        idProduto = produto.idProduto,
                        quantidade = itemDTO.quantidade,
                        vlrUnitario = produto.precoAtual,
                        vlrTotalItem = totalItem
                    };

                    totalVenda += totalItem;

                    db.VendaItems.InsertOnSubmit(vendaItem);
                }

                venda.vlrTotalVenda = totalVenda;

                db.SubmitChanges();

                var response = new
                {
                    venda.idVenda,
                    venda.idCliente,
                    venda.dthRegistro,
                    venda.vlrTotalVenda
                };

                return Content(HttpStatusCode.Created, response);
            }
        }




        /// <summary>
        /// Altera uma venda pelo Id
        /// </summary>
        /// <param name="idVenda"></param>
        /// <param name="vendaDTO"></param>
        [HttpPut]
        [Route("{idVenda:int}")]
        public IHttpActionResult Put(int idVenda, [FromBody] Venda vendaDTO)
        {
            if (vendaDTO == null)
                return BadRequest("Body da requisição não pode ser vazio");

            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var venda = db.Vendas
                    .FirstOrDefault(v => v.idVenda == idVenda);

                if (venda == null)
                    return Content(HttpStatusCode.NotFound, "Venda não encontrada");

                db.SubmitChanges();
                return Ok("Venda atualizada com sucesso");
            }
        }

        /// <summary>
        /// Deleta uma venda pelo seu id
        /// </summary>
        /// <param name="idCliente"></param>
        [HttpDelete]
        [Route("{idVenda:int}")]
        public IHttpActionResult Delete(int idVenda)
        {
            var conn = ConfigurationManager
    .ConnectionStrings["TesteCamposDealerConnectionString"]
    .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var venda = db.Vendas
                    .FirstOrDefault(v => v.idVenda == idVenda);

                if (venda == null)
                    return Content(HttpStatusCode.NotFound, "Venda não encontrada");

                db.Vendas.DeleteOnSubmit(venda);
                db.SubmitChanges();

                return Ok("Venda removida com sucesso");
            }
        }
    }
}
