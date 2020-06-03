using NUnit.Framework;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    [TestFixture]
    public partial class ProductSpecs
    {
        [Test]
        public void product_codes_can_be_used_to_lookup_products()
        {
            isolate(() => { 
                Given(a_product_in_a_group);
                And(a_drug_with_the_same_product_code);
                And(a_drug_terminology);
                When(looking_up_the_product_from_the_drug);
                Then(the_product_reference_is_provided);
                And(the_product_group_is_provided);
            });

            isolate(() => {
                Given(a_product_with_no_group);
                And(a_drug_with_the_same_product_code);
                And(a_drug_terminology);
                When(looking_up_the_product_from_the_drug);
                Then(the_product_reference_is_provided);
                And(the_product_group_is_provided);
            });
        }

        [Test]
        public void product_codes_under_another_terminology_dont_get_returned()
        {
            Given(a_product_in_a_group);
            And(a_drug_with_the_same_product_code);
            And(an_other_drug_terminology);
            When(looking_up_the_product_from_the_drug);
            Then(results_do_not_include_the_drug);
        }

        [Test]
        public void product_codes_dont_match()
        {
            Given(a_product_in_a_group);
            And(a_drug_with_the_different_product_code);
            And(a_drug_terminology);
            When(looking_up_the_product_from_the_drug);
            Then(results_do_not_include_the_drug);
        }

        [Test]
        public void products_can_be_deleted()
        {
            Given(a_product_in_a_group);
            And(a_drug_with_the_same_product_code);
            And(a_drug_terminology);
            And(the_product_is_deleted);
            When(looking_up_the_product_from_the_drug);
            Then(results_do_not_include_the_drug);
        }

        [Test]
        public void results_have_unique_product_names()
        {
            Given(a_product_in_a_group);
            And(a_drug_with_the_same_product_code);
            And(a_drug_with_the_same_product_code);
            And(a_drug_terminology);
            When(looking_up_the_product_from_the_drug);
            Then(the_product_reference_is_provided_only_once);
        }
    }
}
