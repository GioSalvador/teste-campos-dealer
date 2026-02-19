using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using TesteCamposDealer.DB;

[RoutePrefix("api/cliente")]
public class ClienteController : ApiController
{
    private readonly string conn = System.Configuration.ConfigurationManager
                                   .ConnectionStrings["TesteCamposDealerConnectionString"]
                                   .ConnectionString;

    /// <summary>
    /// Recupera todos os clientes
    /// </summary>
    [HttpGet]
    [Route("")]
    public IHttpActionResult GetAll()
    {
        using (var db = new DBTesteCamposDealerDataContext(conn))
        {
            var clientes = db.Clientes
                .Select(c => new
                {
                    idCliente = c.idCliente,
                    nomeCliente = c.nomeCliente,
                    endereco = c.endereco,
                    dthRegistro = c.dthRegistro,
                    Vendas = c.Vendas.Select(v => new
                    {
                        idVenda = v.idVenda,
                        dthRegistro = v.dthRegistro,
                        vlrTotalVenda = v.vlrTotalVenda,
                        VendaItems = v.VendaItems.Select(vi => new
                        {
                            idVendaItem = vi.idVendaItem,
                            Produto = vi.Produto.dscProduto,
                            quantidade = vi.quantidade,
                            vlrTotalItem = vi.vlrTotalItem
                        }).ToList()
                    }).ToList()
                })
                .ToList();

            return Ok(clientes);
        }
    }

    /// <summary>
    /// Recupera cliente por ID
    /// </summary>
    [HttpGet]
    [Route("{idCliente:int}")]
    public IHttpActionResult GetById(int idCliente)
    {
        using (var db = new DBTesteCamposDealerDataContext(conn))
        {
            var cliente = db.Clientes
                .Where(c => c.idCliente == idCliente)
                .Select(c => new
                {
                    idCliente = c.idCliente,
                    nomeCliente = c.nomeCliente,
                    endereco = c.endereco,
                    dthRegistro = c.dthRegistro,
                    Vendas = c.Vendas
                        .OrderByDescending(v => v.dthRegistro)
                        .Select(v => new
                        {
                            idVenda = v.idVenda,
                            dthRegistro = v.dthRegistro,
                            vlrTotalVenda = v.vlrTotalVenda,
                            VendaItems = v.VendaItems.Select(vi => new
                            {
                                idVendaItem = vi.idVendaItem,
                                idProduto = vi.idProduto,
                                Produto = vi.Produto.dscProduto, 
                                quantidade = vi.quantidade,
                                vlrUnitario = vi.vlrUnitario,
                                vlrTotalItem = vi.vlrTotalItem
                            }).ToList()
                        }).ToList()
                })
                .FirstOrDefault();

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    [Route("")]
    public IHttpActionResult Post([FromBody] ClienteCreateDTO dto)
    {
        if (dto == null)
            return BadRequest("Body da requisição não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(dto.nomeCliente))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.endereco))
            return BadRequest("Endereço é obrigatório.");

        try
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var cliente = new Cliente
                {
                    nomeCliente = dto.nomeCliente,
                    endereco = dto.endereco,
                    dthRegistro = DateTime.Now 
                };

                db.Clientes.InsertOnSubmit(cliente);
                db.SubmitChanges();

                return Content(HttpStatusCode.Created, cliente);
            }
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }

    /// <summary>
    /// Atualiza cliente existente
    /// </summary>
    [HttpPut]
    [Route("{idCliente:int}")]
    public IHttpActionResult Put(int idCliente, [FromBody] ClienteUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest("Body da requisição não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(dto.nomeCliente))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.endereco))
            return BadRequest("Endereço é obrigatório.");

        try
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var cliente = db.Clientes
                                .FirstOrDefault(c => c.idCliente == idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound,
                                   new { message = "Cliente não encontrado." });

                cliente.nomeCliente = dto.nomeCliente;
                cliente.endereco = dto.endereco;

                db.SubmitChanges();

                return Ok(cliente);
            }
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }


    /// <summary>
    /// Remove cliente por ID
    /// </summary>
    [HttpDelete]
    [Route("{idCliente:int}")]
    public IHttpActionResult Delete(int idCliente)
    {
        try
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var cliente = db.Clientes
                                .FirstOrDefault(c => c.idCliente == idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound,
                                   new { message = "Cliente não encontrado." });

                db.Clientes.DeleteOnSubmit(cliente);
                db.SubmitChanges();

                return Ok(new { message = "Cliente removido com sucesso." });
            }
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }
}
