using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    public static class DeleteSaleHandlerTestData
    {
        private static readonly Faker<DeleteSaleCommand> DeleteSaleFaker = new Faker<DeleteSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.Guid());

        public static DeleteSaleCommand GenerateDeleteSaleCommand()
        {
            return DeleteSaleFaker.Generate();
        }
    }
}
