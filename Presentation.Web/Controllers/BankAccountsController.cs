﻿using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Core.DomainModel;
using Core.DomainServices;

namespace OS2Indberetning.Controllers
{
    public class BankAccountsController : BaseController<BankAccount>
    {
        public BankAccountsController(IGenericRepository<BankAccount> repo) : base(repo){}
        
        //GET: odata/BankAccounts
        [EnableQuery]
        public IQueryable<BankAccount> Get(ODataQueryOptions<BankAccount> queryOptions)
        {
            var res =  GetQueryable(queryOptions);
            return res;
        }

        //GET: odata/BankAccounts(5)
        public IQueryable<BankAccount> Get([FromODataUri] int key, ODataQueryOptions<BankAccount> queryOptions)
        {
            return GetQueryable(key, queryOptions);
        }

        //PUT: odata/BankAccounts(5)
        public new IHttpActionResult Put([FromODataUri] int key, Delta<BankAccount> delta)
        {
            return base.Put(key, delta);
        }

        //POST: odata/BankAccounts
        [EnableQuery]
        public new IHttpActionResult Post(BankAccount BankAccount)
        {
            return base.Post(BankAccount);
        }

        //PATCH: odata/BankAccounts(5)
        [EnableQuery]
        [AcceptVerbs("PATCH", "MERGE")]
        public new IHttpActionResult Patch([FromODataUri] int key, Delta<BankAccount> delta)
        {
            return base.Patch(key, delta);
        }

        //DELETE: odata/BankAccounts(5)
        public new IHttpActionResult Delete([FromODataUri] int key)
        {
            return base.Delete(key);
        }
    }
}