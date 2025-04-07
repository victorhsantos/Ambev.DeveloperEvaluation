using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Customer(string fullName, string email)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Email = email;

            new CustomerValidator().ValidateAndThrow(this);
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
    }
}
