using System;
using System.Collections.Generic;
using System.Text;

namespace DBO
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Result() { }
        public Result(bool isSuccess, string message, string error, T? data = default)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Error = error;
            this.Data = data;
        }
        public static Result<T> Success(T data, string message = "Success") =>
            new Result<T> { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Failure(string error, string message = "Failure") =>
            new Result<T> { IsSuccess = false, Error = error, Message = message };

    }
}
