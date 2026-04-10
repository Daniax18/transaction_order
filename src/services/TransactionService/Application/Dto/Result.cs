namespace TransactionService.Application.Dto
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Value { get; set; }

        private Result(bool isSucess, T value, string message)
        {
            IsSuccess = isSucess;
            Value = value;
            Message = message;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(true, value, "Success");
        }

        public static Result<T> NOk(string message)
        {
            return new Result<T>(false, default(T)!, message);
        }
    }
}
