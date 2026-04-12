using System.Transactions;
using TransactionService.Domain.Enum;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Models
{
    public class Transaction
    {
        public string Id { get; private set; }
        public string OwnerId { get; private set; } = string.Empty;
        public string ReceiverId { get; private set; } = string.Empty;
        public decimal Amount { get; private set; }
        public TransactionOrderStatus Status { get; set; }
        public int validity { get; private set; }   // TODO : Change to UpperCase
        public DateTime createdAt { get; private set; } = DateTime.UtcNow;
        public DateTime expiredAt { get; private set; }
        public DateTime? updatedStatusAt { get; set; }

        public Transaction() { }

        public Transaction(
            string ownerId, 
            string receiverId, 
            decimal amount, 
            int validity
        )
        {
            Id = Guid.NewGuid().ToString();
            OwnerId = ownerId;
            ReceiverId = receiverId;
            UpdateAmount(amount);
            Status = TransactionOrderStatus.PENDING;
            UpdateValidity(validity);
            createdAt = DateTime.UtcNow;
            expiredAt = createdAt.AddMonths(validity);
            updatedStatusAt = null;
        }

        private void UpdateAmount(decimal newAmount)
        {
            if(newAmount <= 0)
            {
                throw new InvalidAmountException();
            }
            Amount = newAmount;
        }

        private void UpdateValidity(int newValidity)
        {
            if(newValidity <= 0)
            {
                throw new InvalidValidityException();
            }
            validity = newValidity;
        }
    }
}
