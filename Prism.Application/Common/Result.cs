namespace Prism.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public ErrorCode ErrorCode { get; }

        protected Result(bool isSuccess, string? error, ErrorCode errorCode)
        {
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            Error = error;
        }

        public static Result Ok() => new Result(true, null, ErrorCode.None);
        public static Result Fail(string error, ErrorCode errorCode = ErrorCode.Unknown) => new Result(false, error, errorCode);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }


        protected Result(bool isSuccess, T? value, string? error, ErrorCode errorCode) : base(isSuccess, error, errorCode)
        {
            Value = value;
        }


        public static Result<T> Ok(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new Result<T>(true, value, null, ErrorCode.None);
        }

        public new static Result<T> Fail(string error, ErrorCode errorCode = ErrorCode.Unknown) => new Result<T>(false, default, error, errorCode);
    }
}
