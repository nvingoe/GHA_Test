using System.Collections.Generic;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public class ConditionTermBuilder
    {
        private ConditionTermId term = 1;
        private IEnumerable<ClinicalCode> conditions = new List<ClinicalCode> { new ClinicalCode("2038946", ClinicalTerminology.SNoMed) };

        public ConditionTerm Build()
        {
            return new ConditionTerm(term, $"Condition Term {term}", conditions);
        }

        public ConditionTermBuilder For(ConditionTermId term)
        {
            this.term = term;
            return this;
        }

        public ConditionTermBuilder WithConditions(params ClinicalCode[] conditions)
        {
            this.conditions = conditions;
            return this;
        }
    }
}