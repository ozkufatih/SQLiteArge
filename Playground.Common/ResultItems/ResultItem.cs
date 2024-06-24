namespace Playground.Common.ResultItems
{
    public class ResultItem<T>
    {
        public bool IsSuccess { get; private set; }

        public string? Message { get; private set; }

        public T? Data { get; private set; }

        private ResultItem(bool isSuccess, T data, string message)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        public static ResultItem<T> Success(T data = default, string message = "Operation successful.")
        {
            return new ResultItem<T>(true, data, message);
        }

        public static ResultItem<T> Failure(string message)
        {
            T? defaultData = default;
            return new ResultItem<T>(false, defaultData, message);
        }
    }
}
