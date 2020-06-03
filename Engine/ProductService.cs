using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fdb.Rx.Persistence.Dapper;
using Fdb.Rx.Platform;
using RxProduct = Fdb.Rx.Product;

namespace Fdb.Rx.AreasOfInterest
{
    public class ProductService
    {
        private IDb Db { get; }

        public ProductService(IDb db)
        {
            Db = db;
        }
        
        public Product[] LookupProduct(IEnumerable<Drug> drugs, DrugTerminology terminology)
        {
            return Db.Query<Product>("GetProducts", new { ProductCodes = drugs.ToTable(), DrugTerminology = terminology }, CommandType.StoredProcedure).ToArray();
        }
    }

    public class Product
    {
        public RxProduct.Reference Reference { get; private set; }
        public RxProduct.Grouping Group { get; private set; }

        internal Product(string code, string name, DrugTerminology terminology, long? groupId, string groupName)
        {
            Reference = new RxProduct.Reference(code, name, terminology);
            Group = groupId.HasValue ? new RxProduct.Grouping(groupId.Value , groupName) : RxProduct.Grouping.Empty;
        }
    }
}