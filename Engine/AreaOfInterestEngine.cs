using System.Data;
using System.Linq;
using Fdb.Rx.Persistence.Dapper;
using Fdb.Rx.Platform;

namespace Fdb.Rx.AreasOfInterest
{
    internal class AreaOfInterestEngine
    {
        private IDb Db { get; }

        internal AreaOfInterestEngine(IDb db)
        {
            Db = db;
        }

        public ConditionGroupResult Run(Patient patient)
        {
            return ResultFactory.Create(Db.Query<dynamic>("Analyse", new
            {
                ClinicalEntries = patient.ClinicalEntries.Where(c => !c.Started.IsFuture()).ToTable(),
                patient.ClinicalTerminology
            }, CommandType.StoredProcedure));
        }
    }
}