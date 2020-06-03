using System.Configuration;
using Fdb.Rx.Test.Dapper;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public static class Settings
    {
        public static class Database
        {
            private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AreasOfInterest"].ConnectionString;

            public static TestDbUser Retrieval = new TestDbUser(ConnectionString, "AreasOfInterest_Retrieval", "fgT4Dwsh!fY", "AreasOfInterestRetrieval");
            public static TestDbUser Management = new TestDbUser(ConnectionString, "AreasOfInterest_Management", "je7F!juo3f", "AreasOfInterestManagement");
        }
    }
}