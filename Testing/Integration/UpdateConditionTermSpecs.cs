using NUnit.Framework;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    [TestFixture]
    public partial class UpdateConditionTermSpecs
    {
        [Test]
        public void patients_are_not_in_an_aoi_when_their_conditions_are_removed()
        {
            Given(an_area_of_interest);
            And(a_patient_having_a_condition_in_the_aoi);
            When(removing_the_condition_from_the_aoi);
            And(running_the_engine);
            Then(the_patient_is_not_in_the_aoi);
        }

        [Test]
        public void patients_are_in_an_aoi_when_their_condition_is_added()
        {
            Given(an_area_of_interest);
            And(a_patient_having_a_condition_not_in_the_aoi);
            When(adding_the_condition_from_the_aoi);
            And(running_the_engine);
            Then(the_patient_is_in_the_aoi);
        }

        [Test]
        public void patients_are_not_in_an_aoi_when_its_term_is_deleted()
        {
            Given(an_area_of_interest);
            And(a_patient_having_a_condition_in_the_aoi);
            When(deleting_the_condition_term);
            And(running_the_engine);
            Then(the_patient_is_not_in_the_aoi);
        }

        [Test]
        public void patients_remain_in_an_aoi_when_a_differrent_aois_term_is_updated()
        {
            Given(an_area_of_interest);
            And(a_patient_having_a_condition_in_the_aoi);
            And(another_aoi);
            When(updating_the_other_aoi);
            And(running_the_engine);
            Then(the_patient_is_in_the_aoi);
        }
    }
}