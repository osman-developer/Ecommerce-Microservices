
namespace ECommerce.Common.Response
{
    public record Response<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        public static Response<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static Response<T> Fail(string message) =>
            new() { Success = false, Data = default, Message = message };
    }

}
