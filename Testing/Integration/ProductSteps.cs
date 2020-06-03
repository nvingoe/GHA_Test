using System;
using System.Collections.Generic;
using System.Linq;
using Fdb.Rx.AreasOfInterest.Testing.Builders;
using Fdb.Rx.Platform;
using Fdb.Rx.Test.Dapper;
using RxProduct = Fdb.Rx.Product;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public partial class ProductSpecs : DbSpecification
    {
        private RxProduct the_product;
        private List<Drug> the_drugs;
        private Product[] the_results;
        private DrugTerminology the_drug_terminology;
        private const string a_product_code = "1234";

        protected override void before_each()
        {
            base.before_each();
            the_product = RxProduct.Empty;
            the_drugs = new List<Drug>();
            the_results = null;
        }

        private void a_product_in_a_group()
        {
            the_product = new ProductBuilder().WithGroup((new RxProduct.Grouping(123546, "A Product Group"))).WithCode(a_product_code).Build().Save();
        }

        private void a_product_with_no_group()
        {
            the_product = new ProductBuilder().WithGroup(RxProduct.Grouping.Empty).WithCode(a_product_code).Build().Save();
        }

        private void a_drug_with_the_same_product_code()
        {
            the_drugs.Add(new Drug(a_product_code, Drug.CourseDetail.Empty, Drug.AuthorisationDetail.Empty));
        }

        private void a_drug_with_the_different_product_code()
        {
            the_drugs.Add(new Drug("a diff code...", Drug.CourseDetail.Empty, Drug.AuthorisationDetail.Empty));
        }

        private void a_drug_terminology()
        {
            the_drug_terminology = the_product.References.First().Terminology;
        }

        private void an_other_drug_terminology()
        {
            the_drug_terminology = (DrugTerminology)99;
        }

        private void looking_up_the_product_from_the_drug()
        {
            the_results = new ProductService(Settings.Database.Retrieval.Connection).LookupProduct(the_drugs, the_drug_terminology);
        }

        private void the_product_reference_is_provided()
        {
            the_results.Single().Reference.Code.should_be(the_product.References.First().Code);
            the_results.Single().Reference.Name.should_be(the_product.References.First().Name);
            the_results.Single().Reference.Terminology.should_be(the_drug_terminology);
        }

        private void the_product_group_is_provided()
        {
            the_results.Single().Group.should_be(the_product.Group);
        }

        private void the_product_reference_is_provided_only_once()
        {
            the_product_reference_is_provided();
        }

        private void the_product_is_deleted()
        {
            the_product.Delete();
        }

        private void results_do_not_include_the_drug()
        {
            the_results.should_be_empty();
        }
    }
}
