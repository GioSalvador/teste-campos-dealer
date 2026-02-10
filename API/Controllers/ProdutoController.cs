using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using TesteCamposDealer.DB;

namespace TesteCamposDealer.Controllers
{
    public class ProdutoController : ApiController
    {
        /// <summary>
        /// Recupera o Produto por Id..
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetById")]
        public Produto GetById(int idProduto)
        {
            Produto produtoRet = null;

            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                produtoRet = (from c in db.Produtos
                          where c.idProduto == idProduto
                          select c).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return produtoRet;
        }

        /// <summary>
        /// Recupera todos os Produtos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Produto> GetAll()
        {
            List<Produto> lstProdutoRet = null;
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                lstProdutoRet = (from c in db.Produtos
                             select c).ToList();
            }
            catch
            {
                throw;
            }

            return lstProdutoRet;
        }


        /// <summary>
        /// Cadastra um Produto
        /// </summary>
        /// <param name="produtoDTO"></param>
        [HttpPost]
        public bool Post([FromBody] Produto produtoDTO)
        {
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                db.Produtos.InsertOnSubmit(produtoDTO);
                db.SubmitChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Altera um Produto pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        [ActionName("PutById")]
        public bool Put(int idProduto, [FromBody] Produto produtoDTO)
        {
            Produto prodRet = null;

            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                prodRet = (from c in db.Produtos
                           where c.idProduto == idProduto
                           select c).FirstOrDefault();

                if (prodRet == null)
                    return false;

                prodRet.dscProduto = produtoDTO.dscProduto;
                
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Deleta um Produto pelo seu id
        /// </summary>
        /// <param name="idProduto"></param>
        [HttpDelete]
        [ActionName("DeleteById")]
        public bool Delete(int idProduto)
        {
            Produto prodRet = null;

            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                prodRet = (from c in db.Produtos
                           where c.idProduto == idProduto
                           select c).FirstOrDefault();

                if (prodRet == null)
                    return false;

                db.Produtos.DeleteOnSubmit(prodRet);
                db.SubmitChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}