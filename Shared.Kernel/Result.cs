using System;
using System.Collections.Generic;

namespace Shared.Kernel
{
    /// <summary>
    /// نتیجه استاندارد برای پاسخ‌های API
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public IEnumerable<string> Errors { get; }

        protected Result(bool isSuccess, string message, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
        }

        public static Result Success(string message = null)
        {
            return new Result(true, message, null);
        }

        public static Result Failure(string message)
        {
            return new Result(false, message, new[] { message });
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, "عملیات با خطا مواجه شد.", errors);
        }
    }

    /// <summary>
    /// نتیجه استاندارد برای پاسخ‌های API با داده
    /// </summary>
    public class Result<T> : Result
    {
        public T Data { get; }

        protected Result(T data, bool isSuccess, string message, IEnumerable<string> errors)
            : base(isSuccess, message, errors)
        {
            Data = data;
        }

        public static Result<T> Success(T data, string message = null)
        {
            return new Result<T>(data, true, message, null);
        }

        public static new Result<T> Failure(string message)
        {
            return new Result<T>(default, false, message, new[] { message });
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(default, false, "عملیات با خطا مواجه شد.", errors);
        }
    }
}