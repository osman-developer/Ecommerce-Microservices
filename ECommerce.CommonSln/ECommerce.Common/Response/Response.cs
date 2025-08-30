namespace ECommerce.Common.Response
{
    public record Response<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        // Success with data
        public static Response<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        // Success without data
        public static Response<T> Ok(string message = "") =>
            new() { Success = true, Data = default, Message = message };

        // Failure
        public static Response<T> Fail(string message) =>
            new() { Success = false, Data = default, Message = message };
    }


    // Represents "no value"
    public sealed record Unit
    {
        public static readonly Unit Value = new();
    }
}
