using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
//using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using TesteCamposDealer.DB;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Controllers
{
    public class VendaController : ApiController
    {
        /// <summary>
        /// Recupera a Venda por Id..
        /// </summary>
        /// <param name="idVenda"></param>
        /// <returns></returns>
        [System.Web.Http.ActionName("GetById")]
        public Venda GetById(int idVenda)
        {
            Venda vendaRet = null;
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                vendaRet = (from c in db.Vendas
                          where c.idVenda == idVenda
                          select c).FirstOrDefault();
            }
            catch 
            {
                throw;
            }

            return vendaRet;
        }

        /// <summary>
        /// Recupera todas as vendas
        /// </summary>
        /// <returns></returns>
        public List<Venda> GetAll()
        {
            List<Venda> lstVendaret = null;
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                lstVendaret = (from c in db.Vendas
                             select c).ToList();
            }
            catch 
            {
                throw;
            }

            return lstVendaret;
        }


        /// <summary>
        /// Cadastra uma venda
        /// </summary>
        /// <param name="cliente"></param>

        [System.Web.Http.HttpPost]
        public bool Post([FromBody] TesteCamposDealer.Models.VendaVM vendaDTO)
        {
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;
            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);

            try
            {
                
                Venda novaVenda = new Venda();
                novaVenda.idCliente = vendaDTO.idCliente;
                novaVenda.dthRegistro = DateTime.Now;

                db.Vendas.InsertOnSubmit(novaVenda);
                db.SubmitChanges(); 

                foreach (var item in vendaDTO.Itens)
                {
                    VendaItem novoItem = new VendaItem();
                    novoItem.idVenda = novaVenda.idVenda;
                    novoItem.idProduto = item.idProduto;
                    novoItem.quantidade = item.quantidade;
                    novoItem.vlrUnitario = item.vlrUnitario;

                    db.VendaItems.InsertOnSubmit(novoItem);
                }

                db.SubmitChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Altera uma venda pelo Id
        /// </summary>
        /// <param name="idVenda"></param>
        /// <param name="vendaDTO"></param>
        //[System.Web.Http.ActionName("PutById")]
        //public bool Put(int idVenda, [FromBody] Venda vendaDTO)
        //{
        //    Venda vendaRet = null;

        //    DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext();
        //    db.DeferredLoadingEnabled = false;

        //    try
        //    {
        //        vendaRet = (from c in db.Venda
        //                  where c.idVenda == idVenda
        //                  select c).FirstOrDefault();

        //        vendaRet.vlrProduto = vendaDTO.vlrProduto;
        //        db.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return true;
        //}

        /// <summary>
        /// Deleta uma venda pelo seu id
        /// </summary>
        /// <param name="idCliente"></param>
        [System.Web.Http.ActionName("DeleteById")]
        public bool Delete(int idVenda)
        {
            Venda vendaRet = null;
            var conn = ConfigurationManager
            .ConnectionStrings["TesteCamposDealerConnectionString"]
            .ConnectionString;

            DBTesteCamposDealerDataContext db = new DBTesteCamposDealerDataContext(conn);
            db.DeferredLoadingEnabled = false;

            try
            {
                vendaRet = (from c in db.Vendas
                          where c.idVenda == idVenda
                          select c).FirstOrDefault();

                db.Vendas.DeleteOnSubmit(vendaRet);
                db.SubmitChanges();
            }
            catch 
            {
                throw;
            }

            return true;
        }
    }
}
