using System;
using System.Linq;
using Fdb.Rx.Persistence.Dapper;
using Fdb.Rx.Test.Dapper;
using NodaTime;

namespace Fdb.Arx.Reporting.Testing
{
    public partial class GoalMetOpportunitySpSpecs : DbSpecification
    {
        private readonly IDb reporting_management = Settings.Database.Management.Connection;
        private readonly IDb reporting_retrieval = Settings.Database.Retrieval.Connection;
        private TakenOpportunity goalMetOpportunity;
        private Guid opportunityId1 = new Guid();
        private Guid opportunityId2 = new Guid();
        private Guid opportunityId3 = new Guid();
        private string practiceCode = "A00002";

        private void some_goal_met_opportunity_test_data()
        {
            var ruleId = 64;
            goalMetOpportunity = new TakenOpportunity(practiceCode, Date.Now(), opportunityId1, ruleId);
            new GoalMetOpportunityHandler(reporting_management).Execute(goalMetOpportunity);

            ruleId = 65;
            goalMetOpportunity = new TakenOpportunity(practiceCode, Date.Now().PlusDays(-7), opportunityId2, ruleId);
            new GoalMetOpportunityHandler(reporting_management).Execute(goalMetOpportunity);

            goalMetOpportunity = new TakenOpportunity(practiceCode, Date.Now(), opportunityId3, ruleId);
            new GoalMetOpportunityHandler(reporting_management).Execute(goalMetOpportunity);
        }

        private void calling_the_sp()
        {

        }


        private void the_right_data_is_returned()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var todayPlusOne = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            var sevenDayAgo = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var sevenDayAgoPlusOne = DateTime.Now.AddDays(-7).AddDays(1).ToString("yyyy-MM-dd");

            var actual = reporting_retrieval.Query<int>("exec dbo.GoalMetOpportunities '1900-01-01', '2100-01-01', 65, null").Single();
            actual.should_be(2);

            actual = reporting_retrieval.Query<int>("exec dbo.GoalMetOpportunities '1900-01-01', '2100-01-01', 64, null").Single();
            actual.should_be(1);

            actual = reporting_retrieval.Query<int>("exec dbo.GoalMetOpportunities '1900-01-01', '2100-01-01', null, null").Single();
            actual.should_be(3);

            actual = reporting_retrieval.Query<int>($"exec dbo.GoalMetOpportunities '{today}', '{todayPlusOne}', null, null").Single();
            actual.should_be(2);

            actual = reporting_retrieval.Query<int>($"exec dbo.GoalMetOpportunities '{sevenDayAgo}', '{sevenDayAgoPlusOne}', null, null").Single();
            actual.should_be(1);

            actual = reporting_retrieval.Query<int>("exec dbo.GoalMetOpportunities '1900-01-01', '2100-01-01', null, 'Test'").Single();
            actual.should_be(0);
        }
    }
}
