using System;
using System.Configuration;
using System.Reflection;
using DbUp;

namespace Fdb.Arx.Opportunity
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AreasOfInterest.Migration.Connection"].ConnectionString;
            EnsureDatabase.For.SqlDatabase(connectionString);
            
            var result = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransactionPerScript()
                .LogToConsole()
                .Build()
                .PerformUpgrade(); 

            if (!result.Successful)
                Console.WriteLine(result.Error);

            return result.Successful ? 0 : -1;
        }
    }
}
