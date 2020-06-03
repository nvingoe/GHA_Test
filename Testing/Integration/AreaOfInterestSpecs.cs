using NUnit.Framework;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    [TestFixture]
    public partial class AreaOfInterestSpecs
    {
        [Test]
        public void patient_with_matching_condition_is_in_aoi()
        {
            Given(an_area_of_interest);
            And(a_patient_that_has_a_condition_in_the_aoi);
            When(running_the_aoi_engine);
            Then(the_patient_is_in_the_aoi);
        }

        [Test]
        public void patient_whose_condition_no_longer_matches_is_not_in_aoi()
        {
            Given(an_area_of_interest);
            And(an_update_to_the_area_of_interest);
            And(a_patient_that_has_a_condition_in_the_original_term_but_not_in_the_revised_term);
            When(running_the_aoi_engine);
            Then(patient_results_no_longer_in_aoi);
        }

        [Test]
        public void patient_whose_condition_now_matches_is_in_new_aoi()
        {
            Given(an_area_of_interest);
            And(an_update_to_the_area_of_interest);
            And(a_patient_that_has_a_condition_in_the_revised_term_only);
            When(running_the_aoi_engine);
            Then(patient_results_include_new_aoi);
        }

        [Test]
        public void condition_driven_aois_can_be_deleted()
        {
            Given(an_area_of_interest);
            And(a_patient_that_matches);
            When(deleted);
            Then(the_patient_no_longer_matches);
        }

        [Test]
        public void results_from_an_existing_aoi_remain_when_a_new_aoi_is_added()
        {
            Given(an_area_of_interest);
            And(a_patient_that_matches);
            And(a_new_area_of_interest);
            When(running_the_aoi_engine);
            Then(the_patient_is_in_the_aoi);
        }
    }
}
