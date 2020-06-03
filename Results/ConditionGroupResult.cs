namespace Fdb.Rx.AreasOfInterest
{
    public class ConditionGroupResult
    {
        public AreaOfInterestMatch[] AreaOfInterestMatches { get; private set; }
        public ConditionFilterMatch[] ConditionFilterMatches { get; private set; }

        public ConditionGroupResult(AreaOfInterestMatch[] areaOfInterestMatches, ConditionFilterMatch[] conditionFilterMatches)
        {

            AreaOfInterestMatches = areaOfInterestMatches;
            ConditionFilterMatches = conditionFilterMatches;
        }
    }
}
