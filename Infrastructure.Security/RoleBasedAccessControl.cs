using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Security
{
    /// <summary>
    /// کنترل دسترسی بر اساس نقش (Role-Based Access Control)
    /// </summary>
    public class RoleBasedAccessControl
    {
        private readonly Dictionary<string, List<string>> _rolePermissions;

        public RoleBasedAccessControl()
        {
            // تعریف مجوزها برای نقش‌ها
            _rolePermissions = new Dictionary<string, List<string>>
            {
                ["User"] = new List<string> { "ViewOwnLoan", "ApplyForLoan", "ViewInstallments" },
                ["Admin"] = new List<string> { "ViewAllLoans", "ApproveLoan", "ManageUsers", "ViewReports" }
            };
        }

        /// <summary>
        /// بررسی می‌کند که آیا نقش کاربر دسترسی به عملیات خاصی دارد
        /// </summary>
        /// <param name="role">نقش کاربر</param>
        /// <param name="permission">عملیات مورد نظر</param>
        /// <returns>درست در صورت داشتن دسترسی</returns>
        public bool HasPermission(string role, string permission)
        {
            if (!_rolePermissions.ContainsKey(role))
                return false;

            return _rolePermissions[role].Contains(permission);
        }

        /// <summary>
        /// بررسی می‌کند که آیا کاربر دارای نقش مورد نیاز است
        /// </summary>
        /// <param name="claimsPrincipal">اطلاعات کاربر از توکن JWT</param>
        /// <param name="requiredRole">نقش مورد نیاز</param>
        /// <returns>درست در صورت داشتن نقش</returns>
        public bool UserHasRole(ClaimsPrincipal claimsPrincipal, string requiredRole)
        {
            return claimsPrincipal.IsInRole(requiredRole);
        }

        /// <summary>
        /// ایجاد پالیسی اجازه دسترسی بر اساس نقش
        /// </summary>
        /// <param name="role">نقش مورد نیاز</param>
        /// <returns>پالیسی اجازه دسترسی</returns>
        public AuthorizationPolicy CreatePolicyForRole(string role)
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(role)
                .Build();
        }
    }
}