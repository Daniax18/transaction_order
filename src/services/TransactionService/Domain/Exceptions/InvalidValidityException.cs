namespace TransactionService.Domain.Exceptions
{
    public class InvalidValidityException : Exception
    {
        public InvalidValidityException() : base("Validity must be greater than zero.")
        { }
    }
}
