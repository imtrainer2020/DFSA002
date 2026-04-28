using System;
using System.Collections.Generic;
using System.Text;

namespace DBO
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Result() { }
        public Result(bool isSuccess, string message, T? data = default)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }
        public static Result<T> Success(T data, string successMessage = "Success") =>
            new Result<T>
            {
                IsSuccess = true,
                Data = data,
                Message = successMessage
            };

        public static Result<T> Failure(string errorMessage) =>
            new Result<T>
            {
                IsSuccess = false,
                Message = errorMessage
            };
    }
}
