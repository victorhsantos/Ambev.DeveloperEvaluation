using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    public static class GetSalesHandlerTestData
    {
        private static readonly Faker<GetSalesQuery> GetSalesFaker = new Faker<GetSalesQuery>()
            .RuleFor(q => q.PageNumber, f => f.Random.Int(1, 10))
            .RuleFor(q => q.PageSize, f => f.Random.Int(1, 100));

        public static GetSalesQuery GenerateGetSalesQuery()
        {
            return GetSalesFaker.Generate();
        }
    }
}

