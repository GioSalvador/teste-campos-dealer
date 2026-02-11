using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using TesteCamposDealer.DB;

namespace TesteCamposDealer.Controllers
{
    public class ClienteController : ApiController
    {
        /// <summary>
        /// Recupera o Cliente por Id..
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cliente/{idCliente:int}")]
        public IHttpActionResult GetById(int idCliente)
        {
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            Cliente cliente;

            try
            {
                cliente = db.Cliente
                    .FirstOrDefault(c => c.idCliente == idCliente);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (cliente == null)
                return Content(HttpStatusCode.NotFound, "Cliente não encontrado");

            return Ok(cliente);
        }

        /// <summary>
        /// Recupera todos os clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cliente")]
        public IHttpActionResult GetAll()
        {
            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            using (var db = new DBTesteCamposDealerDataContext(conn))
            {
                db.DeferredLoadingEnabled = false;
                var clientes = db.Cliente.ToList();
                return Ok(clientes);
            }
        }


        /// <summary>
        /// Cadastra um Cliente
        /// </summary>
        /// <param name="cliente"></param>
        [HttpPost]
        [Route("api/cliente")]
        public IHttpActionResult Post([FromBody] Cliente clienteDTO)
        {

            if (clienteDTO == null)
            {
                return BadRequest("Body da requisição não pode ser vazio");
            }

            if (string.IsNullOrWhiteSpace(clienteDTO.nomeCliente))
            {
                return BadRequest("Nome do cliente é obrigatório");
            }

            if (string.IsNullOrWhiteSpace(clienteDTO.endereco))
            {
                return BadRequest("Endereço do cliente é obrigatório");
            }

            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                clienteDTO.dthRegistro = DateTime.Now;

                db.Cliente.InsertOnSubmit(clienteDTO);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, clienteDTO);
        }

        /// <summary>
        /// Altera um Cliente pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        [Route("api/cliente/{idCliente:int}")]
        public IHttpActionResult PutById(int idCliente, [FromBody] Cliente clienteDTO)
        {
            if (clienteDTO == null)
                return BadRequest("Body da requisição não pode ser vazio");

            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            DBTesteCamposDealerDataContext db =
                new DBTesteCamposDealerDataContext(conn);

            db.DeferredLoadingEnabled = false;

            try
            {
                var cliente = db.Cliente
                    .FirstOrDefault(c => c.idCliente == idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound, "Cliente não encontrado");

                cliente.nomeCliente = clienteDTO.nomeCliente;
                cliente.endereco = clienteDTO.endereco;

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("Cliente atualizado com sucesso");
        }

        /// <summary>
        /// Deleta um cliente pelo seu id
        /// </summary>
        /// <param name="idCliente"></param>
        [HttpDelete]
        [Route("api/cliente/{idCliente:int}")]
        public IHttpActionResult Delete(int idCliente)
        {
            var conn = ConfigurationManager
                .ConnectionStrings["TesteCamposDealerConnectionString"]
                .ConnectionString;

            DBTesteCamposDealerDataContext db =
                new DBTesteCamposDealerDataContext(conn);

            db.DeferredLoadingEnabled = false;

            try
            {
                var cliente = db.Cliente
                    .FirstOrDefault(c => c.idCliente == idCliente);

                if (cliente == null)
                    return Content(HttpStatusCode.NotFound, "Cliente não encontrado");

                db.Cliente.DeleteOnSubmit(cliente);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("Cliente removido com sucesso");
        }
    }
}
