using Fdb.Rx.Primitives.Dapper;
using NUnit.Framework;

namespace Fdb.Rx.AreasOfInterest.Testing
{
    public class Startup
    {
        [SetUpFixture]
        public class TestsSetupClass
        {
            [OneTimeSetUp]
            public void Setup()
            {
                PrimitivesForDapper.Register();
            }
        }
    }
}