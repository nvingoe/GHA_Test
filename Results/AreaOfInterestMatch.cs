using System;

namespace Fdb.Rx.AreasOfInterest
{
    public class AreaOfInterestMatch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public ClinicalCode[] Conditions { get; private set; }

        private AreaOfInterestMatch() { }

        internal AreaOfInterestMatch(Guid id, string name, ClinicalCode[] conditions)
        {
            Id = id;
            Name = name;
            Conditions = conditions;
        }
    }
}