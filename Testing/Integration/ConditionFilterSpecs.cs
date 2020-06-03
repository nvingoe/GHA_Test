using NUnit.Framework;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    [TestFixture]
    public partial class ConditionFilterSpecs
    {
        [Test]
        public void patient_with_matching_condition_is_in_condition_filter()
        {
            Given(a_condition_filter);
            And(a_patient_that_has_a_condition_in_the_condition_filter);
            When(running_the_areas_of_interest_engine);
            Then(the_patient_is_in_the_condition_filter);
        }

        [Test]
        public void patient_whose_condition_no_longer_matches_is_not_in_condition_filter()
        {
            Given(a_condition_filter);
            And(an_update_to_the_condition_filter);
            And(a_patient_that_has_a_condition_in_the_original_term_but_not_in_the_revised_term);
            When(running_the_areas_of_interest_engine);
            Then(patient_results_no_longer_in_condition_filter);
        }

        [Test]
        public void patient_whose_condition_now_matches_is_in_new_condition_filter()
        {
            Given(a_condition_filter);
            And(an_update_to_the_condition_filter);
            And(a_patient_that_has_a_condition_in_the_revised_term_only);
            When(running_the_areas_of_interest_engine);
            Then(patient_results_include_new_condition_filter);
        }

        [Test]
        public void condition_filters_can_be_deleted()
        {
            Given(a_condition_filter);
            And(a_patient_that_matches);
            When(deleted);
            Then(the_patient_no_longer_matches);
        }

        [Test]
        public void results_from_an_existing_condition_filter_remain_when_a_new_condition_filter_is_added()
        {
            Given(a_condition_filter);
            And(a_patient_that_matches);
            And(a_new_condition_filter);
            When(running_the_areas_of_interest_engine);
            Then(the_patient_is_in_the_condition_filter);
        }
    }
}
