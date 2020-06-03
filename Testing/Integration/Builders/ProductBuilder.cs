using System;
using RxProduct = Fdb.Rx.Product;

namespace Fdb.Rx.AreasOfInterest.Testing.Builders
{
    public class ProductBuilder
    {
        private static readonly Random random = new Random();

        private string code = "12345";
        private static readonly RxProduct.Classes productClass = RxProduct.Classes.TheoreticalGeneric;
        private RxProduct.Grouping group = new RxProduct.Grouping(2190009, "Trihexyphenidyl");
        private const string name = "Some Product";

        public ProductBuilder() { }

        public ProductBuilder WithCode(string code)
        {
            this.code = code;
            return this;
        }

        public ProductBuilder WithGroup(RxProduct.Grouping group)
        {
            this.group = group;
            return this;
        }

        public RxProduct Build()
        {
            return new RxProduct(productClass, group, new[]
            {
                new RxProduct.Reference(code, name, DrugTerminology.Multilex), new RxProduct.Reference(code + code, $"{name} - DMD", DrugTerminology.Dmd)
            });
        }
    }

    public static class ProductExtensions
    {
        private static readonly Management management = new Management(Settings.Database.Management.Connection);

        public static RxProduct Save(this RxProduct product)
        {
            management.SaveProduct(product);
            return product;
        }

        public static void Delete(this RxProduct product)
        {
            management.DeleteProduct(product);
        }
    }
}