namespace Nornickel.Common;

public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public bool IsFailed => !IsSuccess;
    public T? Value { get; }
    public Dictionary<string, string[]>? Error { get; }
    public string? Details { get; }

    private ServiceResult(T value) { IsSuccess = true; Value = value; }
    private ServiceResult(Dictionary<string, string[]> error, string details) { IsSuccess = false; Error = error; Details = details; }
    private ServiceResult(string details) { IsSuccess = false; Details = details; }

    public static ServiceResult<T> Ok(T value) => new(value);
    public static ServiceResult<T> Fail(Dictionary<string, string[]> error, string details) => new(error, details);
    public static ServiceResult<T> Fail(string details) => new(details);
}
