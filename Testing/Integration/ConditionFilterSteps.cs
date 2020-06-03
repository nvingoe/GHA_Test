using System;
using System.Linq;
using Fdb.Rx.Persistence.Dapper;
using Fdb.Rx.Platform;
using Fdb.Rx.Test.Dapper;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public partial class ConditionFilterSpecs : DbSpecification
    {
        private readonly IDb areasOfInterest_management = Settings.Database.Management.Connection;
        private readonly IDb areasOfInterest_retrieval = Settings.Database.Retrieval.Connection;

        private Guid id = Guid.NewGuid();
        private string name = "my Condition Filter";
        private string other_name = "updated Condition Filter";
        private ConditionTerm term;
        
        private Patient the_patient;
        private ClinicalCode the_condition;
        private ClinicalCode the_other_condition;
        private ConditionGroupResult theConditionGroupResults;
        
        protected override void before_each()
        {
            the_patient = null;
            the_condition = new ClinicalCode("wibble", ClinicalTerminology.SNoMed);
            the_other_condition = new ClinicalCode("wobble", ClinicalTerminology.SNoMed);
            term = new ConditionTermBuilder().For(123).WithConditions(the_condition).Build();
            base.before_each();
        }

        private void a_condition_filter()
        {
            new Management(areasOfInterest_management).SaveConditionFilter(id, name, term);
        }

        private void a_new_condition_filter()
        {
            new Management(areasOfInterest_management).SaveConditionFilter(Guid.NewGuid(), name, new ConditionTermBuilder().For(234).WithConditions(new ClinicalCode("tribfboloe", ClinicalTerminology.SNoMed)).Build());
        }

        private void a_patient_that_matches()
        {
            a_patient_that_has_a_condition_in_the_condition_filter();
            running_the_areas_of_interest_engine();
            the_patient_is_in_the_condition_filter();
        }

        private void an_update_to_the_condition_filter()
        {
            term = new ConditionTermBuilder().For(123).WithConditions(the_other_condition).Build();
            new Management(areasOfInterest_management).SaveConditionFilter(id, other_name, term);
        }

        private void a_patient_that_has_a_condition_in_the_condition_filter()
        {
            the_patient = new Patient(Guid.NewGuid(), Vendor.Emis, "SOME-PATIENT-REF", "A0003", Date.Today().PlusYears(-20), Sexes.Both, null, DrugTerminology.Dmd, ClinicalTerminology.SNoMed);
            the_patient.Has(new ClinicalEntry(the_condition.Code, Date.Today()));
        }

        private void a_patient_that_has_a_condition_in_the_original_term_but_not_in_the_revised_term() =>
            a_patient_that_has_a_condition_in_the_condition_filter();

        private void a_patient_that_has_a_condition_in_the_revised_term_only()
        {
            the_patient = new Patient(Guid.NewGuid(), Vendor.Emis, "ANOTHER-PATIENT-REF", "A0003", Date.Today().PlusYears(-20), Sexes.Both, null, DrugTerminology.Dmd, ClinicalTerminology.SNoMed);
            the_patient.Has(new ClinicalEntry(the_other_condition.Code, Date.Today()));
        }

        private void running_the_areas_of_interest_engine()
        {
            theConditionGroupResults = new AreaOfInterestEngine(areasOfInterest_retrieval).Run(the_patient);
        }

        private void deleted()
        {
            new Management(areasOfInterest_management).DeleteConditionFilter(id);
        }

        private void the_patient_is_in_the_condition_filter()
        {
            theConditionGroupResults.ConditionFilterMatches.Single().Id.should_be(id);
            theConditionGroupResults.ConditionFilterMatches.Single().Name.should_be(name);
            theConditionGroupResults.ConditionFilterMatches.Single().Conditions.Single().should_be(the_condition);
        }

        private void patient_results_include_new_condition_filter()
        {
            theConditionGroupResults.ConditionFilterMatches.Single().Id.should_be(id);
            theConditionGroupResults.ConditionFilterMatches.Single().Name.should_be(other_name);
            theConditionGroupResults.ConditionFilterMatches.Single().Conditions.Single().should_be(the_other_condition);
        }

        private void patient_results_no_longer_in_condition_filter()
        {
            theConditionGroupResults.ConditionFilterMatches.should_be_empty();
        }


        private void the_patient_no_longer_matches()
        {
            running_the_areas_of_interest_engine();
            patient_results_no_longer_in_condition_filter();
        }
    }
}
