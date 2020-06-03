using System;
using System.Linq;
using Fdb.Rx.Platform;
using Fdb.Rx.Test.Dapper;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public partial class UpdateConditionTermSpecs : DbSpecification
    {
        private static readonly Guid the_aoi_id = Guid.NewGuid();
        private Patient the_patient;
        private ConditionTerm the_term;

        private static readonly ClinicalCode the_new_condition_code = new ClinicalCode("a new code", ClinicalTerminology.SNoMed);
        private static readonly ConditionTermId the_term_id = 35;
        private static readonly ConditionTermId the_other_term_id = 97;
        private ConditionGroupResult theConditionGroupResults;


        private string an_aoi = "An AOI";
        private string a_patient = "a patient";
        private string practice_code = "A3333";

        protected override void before_each()
        {
            base.before_each();
            the_patient = null;
            the_term = new ConditionTermBuilder().Build();
            theConditionGroupResults = null;
        }

        private void an_area_of_interest()
        {
            new Management(Settings.Database.Management.Connection).Save(the_aoi_id, an_aoi, new ConditionTermBuilder().For(the_term_id).Build());
        }

        private void a_patient_having_a_condition_in_the_aoi()
        {
            the_patient = new Patient(Guid.NewGuid(), Vendor.Emis, a_patient, practice_code, Date.Today().PlusDays(-30), Sexes.Both, null, DrugTerminology.Dmd, ClinicalTerminology.SNoMed);
            the_patient.Has(new ClinicalEntry(the_term.Conditions.First().Code, Date.Today().PlusDays(-10)));
        }

        private void a_patient_having_a_condition_not_in_the_aoi()
        {
            the_patient = new Patient(Guid.NewGuid(), Vendor.Emis, a_patient, practice_code, Date.Today().PlusDays(-30), Sexes.Both, null, DrugTerminology.Dmd, ClinicalTerminology.SNoMed);
            the_patient.Has(new ClinicalEntry(the_new_condition_code.Code, Date.Today().PlusDays(-10)));
        }

        private void another_aoi()
        {
            new Management(Settings.Database.Management.Connection).Save(Guid.NewGuid(), an_aoi, new ConditionTermBuilder().For(the_other_term_id).Build());
        }

        private void updating_the_other_aoi()
        {
            new Management(Settings.Database.Management.Connection).SaveConditionTerm(new ConditionTermBuilder().For(the_other_term_id).WithConditions(the_new_condition_code).Build());
        }

        private void removing_the_condition_from_the_aoi()
        {

            new Management(Settings.Database.Management.Connection).SaveConditionTerm(new ConditionTermBuilder().For(the_term_id).WithConditions(the_new_condition_code).Build());
        }

        private void adding_the_condition_from_the_aoi()
        {
            new Management(Settings.Database.Management.Connection).SaveConditionTerm(new ConditionTermBuilder().For(the_term_id).WithConditions(the_new_condition_code).Build());
        }

        private void deleting_the_condition_term()
        {
            new Management(Settings.Database.Management.Connection).DeleteConditionTerm(the_term_id);
        }

        private void running_the_engine()
        {
            theConditionGroupResults = new AreaOfInterestEngine(Settings.Database.Retrieval.Connection).Run(the_patient);
        }

        private void the_patient_is_not_in_the_aoi()
        {
            theConditionGroupResults.AreaOfInterestMatches.should_be_empty();
        }

        private void the_patient_is_in_the_aoi()
        {
            theConditionGroupResults.AreaOfInterestMatches.Single().Id.should_be(the_aoi_id);
        }
    }
}