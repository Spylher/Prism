namespace Prism.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }


        protected Result(bool isSuccess, string? error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }


        public static Result Ok() => new Result(true);
        public static Result Fail(string error) => new Result(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }


        protected Result(bool isSuccess, T? value, string? error = null)
            : base(isSuccess, error)
        {
            Value = value;
        }


        public static Result<T> Ok(T value) => new(true, value);
        public new static Result<T> Fail(string error) => new(false, default, error);
    }
}
