using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Kernel.Extensions
{
    /// <summary>
    /// متدهای گسترشی برای کار با اشیاء
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// تبدیل شیء به دیکشنری (مثلاً برای لاگ یا ارسال به API)
        /// </summary>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null) return null;

            var dictionary = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var value = property.GetValue(obj);
                    dictionary[property.Name] = value;
                }
                catch
                {
                    dictionary[property.Name] = "Error reading value";
                }
            }

            return dictionary;
        }

        /// <summary>
        /// بررسی می‌کند که آیا شیء خالی است یا نه
        /// </summary>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// بررسی می‌کند که آیا شیء مقدار دارد یا نه
        /// </summary>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// ایمن‌ترین روش برای تبدیل شیء به رشته
        /// </summary>
        public static string SafeToString(this object obj)
        {
            if (obj == null) return "null";
            try
            {
                return obj.ToString();
            }
            catch
            {
                return $"[Object: {obj.GetType().Name}]";
            }
        }

        /// <summary>
        /// بررسی می‌کند که آیا شیء از نوع خاصی است
        /// </summary>
        public static bool IsOfType<T>(this object obj)
        {
            return obj is T;
        }

        /// <summary>
        /// ایمن‌ترین روش برای اجرای عملیات روی شیء (Null-Conditional)
        /// </summary>
        public static void IfNotNull<T>(this T obj, Action<T> action) where T : class
        {
            if (obj != null)
            {
                action(obj);
            }
        }

        /// <summary>
        /// ایمن‌ترین روش برای اجرای عملیات روی شیء و بازگرداندن مقدار (Null-Conditional)
        /// </summary>
        public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> func, TResult defaultValue = default) where T : class
        {
            return obj != null ? func(obj) : defaultValue;
        }

        /// <summary>
        /// تبدیل لیست اشیاء به لیست دیکشنری
        /// </summary>
        public static List<Dictionary<string, object>> ToDictionaryList(this IEnumerable<object> objects)
        {
            if (objects == null) return new List<Dictionary<string, object>>();
            return objects.Select(o => o.ToDictionary()).ToList();
        }
    }
}