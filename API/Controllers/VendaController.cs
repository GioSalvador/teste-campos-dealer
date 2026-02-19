using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using TesteCamposDealer.DB;
using TesteCamposDealer.Models.DTO;

namespace TesteCamposDealer.Controllers
{
    [RoutePrefix("api/venda")]
    public class VendaController : ApiController
    {
        private readonly string conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

        /// <summary>
        /// Recupera todas as vendas
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var vendas = db.Vendas
                    .Select(v => new
                    {
                        v.idVenda,
                        v.idCliente,
                        Cliente = v.Cliente.nomeCliente, 
                        v.dthRegistro,
                        v.vlrTotalVenda,
                        VendaItems = v.VendaItems.Select(vi => new
                        {
                            vi.idVendaItem,
                            vi.idProduto,
                            Produto = vi.Produto.dscProduto, 
                            vi.quantidade,
                            vi.vlrUnitario,
                            vi.vlrTotalItem
                        })
                    })
                    .OrderByDescending(v => v.dthRegistro)
                    .ToList();

                return Ok(vendas);
            }
        }


        /// <summary>
        /// Recupera venda por ID
        /// </summary>
        [HttpGet]
        [Route("{idVenda:int}")]
        public IHttpActionResult GetById(int idVenda)
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var venda = db.Vendas
                    .Where(v => v.idVenda == idVenda)
                    .Select(v => new
                    {
                        v.idVenda,
                        v.idCliente,
                        Cliente = v.Cliente.nomeCliente, 
                        v.dthRegistro,
                        v.vlrTotalVenda,
                        VendaItems = v.VendaItems.Select(vi => new
                        {
                            vi.idVendaItem,
                            vi.idProduto,
                            Produto = vi.Produto.dscProduto, 
                            vi.quantidade,
                            vi.vlrUnitario,
                            vi.vlrTotalItem
                        })
                    })
                    .FirstOrDefault();

                if (venda == null)
                    return Content(HttpStatusCode.NotFound,
                        new { message = "Venda não encontrada." });

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
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var clienteExiste = db.Clientes
                    .Any(c => c.idCliente == idCliente);

                if (!clienteExiste)
                    return Content(HttpStatusCode.NotFound,
                        new { message = "Cliente não encontrado." });

                var vendas = db.Vendas
                    .Where(v => v.idCliente == idCliente)
                    .Select(v => new
                    {
                        v.idVenda,
                        v.idCliente,
                        Cliente = v.Cliente.nomeCliente, 
                        v.dthRegistro,
                        v.vlrTotalVenda,
                        VendaItems = v.VendaItems.Select(vi => new
                        {
                            vi.idVendaItem,
                            vi.idProduto,
                            Produto = vi.Produto.dscProduto,
                            vi.quantidade,
                            vi.vlrUnitario,
                            vi.vlrTotalItem
                        })
                    })
                    .OrderByDescending(v => v.dthRegistro)
                    .ToList();

                return Ok(vendas);
            }
        }


        /// <summary>
        /// Retorna ranking das 10 maiores vendas
        /// </summary>
        [HttpGet]
        [Route("ranking")]
        public IHttpActionResult GetRanking()
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;

                var ranking = db.Vendas
                    .OrderByDescending(v => v.vlrTotalVenda)
                    .Take(10)
                    .Select(v => new
                    {
                        v.idVenda,
                        v.idCliente,
                        v.dthRegistro,
                        v.vlrTotalVenda
                    })
                    .ToList();

                return Ok(ranking);
            }
        }

        /// <summary>
        /// Cadastra uma nova venda
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] VendaDTO vendaDTO)
        {
            if (vendaDTO == null || vendaDTO.itens == null || !vendaDTO.itens.Any())
                return BadRequest("Venda deve conter ao menos um item.");

            try
            {
                using (var db = new DBTesteCamposDealerDataContext(conn))
                {
                    db.DeferredLoadingEnabled = false;

                    var cliente = db.Clientes
                        .FirstOrDefault(c => c.idCliente == vendaDTO.idCliente);

                    if (cliente == null)
                        return Content(HttpStatusCode.NotFound,
                            new { message = "Cliente não encontrado." });

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
                            return Content(HttpStatusCode.NotFound,
                                new { message = $"Produto {itemDTO.idProduto} não encontrado." });

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
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Venda não pode ser alterada após registro
        /// </summary>
        [HttpPut]
        [Route("{idVenda:int}")]
        public IHttpActionResult Put(int idVenda, [FromBody] Venda vendaDTO)
        {
            return BadRequest("Venda não pode ser alterada após registro.");
        }

        /// <summary>
        /// Remove venda por ID
        /// </summary>
        [HttpDelete]
        [Route("{idVenda:int}")]
        public IHttpActionResult Delete(int idVenda)
        {
            try
            {
                using (var db = new DBTesteCamposDealerDataContext(conn))
                {
                    var venda = db.Vendas
                        .FirstOrDefault(v => v.idVenda == idVenda);

                    if (venda == null)
                        return Content(HttpStatusCode.NotFound,
                            new { message = "Venda não encontrada." });

                    db.Vendas.DeleteOnSubmit(venda);
                    db.SubmitChanges();

                    return Ok(new { message = "Venda removida com sucesso." });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
