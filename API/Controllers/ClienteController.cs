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
            db.DeferredLoadingEnabled = false;

            var clientes = db.Clientes.ToList();

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
            db.DeferredLoadingEnabled = false;

            var cliente = db.Clientes
                            .FirstOrDefault(c => c.idCliente == idCliente);

            if (cliente == null)
                return Content(HttpStatusCode.NotFound,
                               new { message = "Cliente não encontrado." });

            return Ok(cliente);
        }
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    [Route("")]
    public IHttpActionResult Post([FromBody] Cliente cliente)
    {
        if (cliente == null)
            return BadRequest("Body da requisição não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(cliente.nomeCliente))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(cliente.endereco))
            return BadRequest("Endereço é obrigatório.");

        try
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
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
    public IHttpActionResult Put(int idCliente, [FromBody] Cliente clienteDTO)
    {
        if (clienteDTO == null)
            return BadRequest("Body da requisição não pode ser vazio.");

        try
        {
            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                var cliente = db.Clientes
                                .FirstOrDefault(c => c.idCliente == idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound,
                                   new { message = "Cliente não encontrado." });

                if (string.IsNullOrWhiteSpace(clienteDTO.nomeCliente))
                    return BadRequest("Nome é obrigatório.");

                if (string.IsNullOrWhiteSpace(clienteDTO.endereco))
                    return BadRequest("Endereço é obrigatório.");

                cliente.nomeCliente = clienteDTO.nomeCliente;
                cliente.endereco = clienteDTO.endereco;

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
