using System;

namespace Fdb.Rx.AreasOfInterest
{
    public class ConditionFilterMatch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ClinicalCode[] Conditions { get; private set; }

        private ConditionFilterMatch() { }

        internal ConditionFilterMatch(Guid id, string name, ClinicalCode[] conditions)
        {
            Id = id;
            Name = name;
            Conditions = conditions;
        }
    }
}