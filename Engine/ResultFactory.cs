using System.Collections.Generic;
using System.Linq;

namespace Fdb.Rx.AreasOfInterest
{
    internal static class ResultFactory
    {
        internal static ConditionGroupResult Create(IEnumerable<dynamic> matches)
        {
            var areaOfInterestMatchesAggregated = matches.Where(m => m.IsAreaOfInterest)
                                        .GroupBy(row => new { row.Id, row.Name })
                                        .Select(g => new AreaOfInterestMatch(g.Key.Id, g.Key.Name, g.Select(r => new ClinicalCode(r.ClinicalCode, (ClinicalTerminology)r.Terminology)).ToArray()))
                                        .ToArray();

            var conditionFilterMatchesAggregated = matches.Where(m => m.IsFilter)
                                        .GroupBy(row => new { row.Id, row.Name })
                                        .Select(g => new ConditionFilterMatch(g.Key.Id, g.Key.Name, g.Select(r => new ClinicalCode(r.ClinicalCode, (ClinicalTerminology)r.Terminology)).ToArray()))
                                        .ToArray();

            return new ConditionGroupResult(areaOfInterestMatchesAggregated, conditionFilterMatchesAggregated);
        }
  }
}