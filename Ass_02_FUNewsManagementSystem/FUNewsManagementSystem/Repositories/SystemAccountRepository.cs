﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using FUNewsManagementSystem.Models;
using FUNewsManagementSystem.ViewModel;
using System.Security.Claims;

namespace FUNewsManagementSystem.Repositories
{

    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetAccounts(out string message);
        void CreateAccount(SystemAccount newAccount, out string message);
        void UpdateAccount(int id, SystemAccount updateAccount, out string message);
        void DeleteAccount(int id, out string message);
        List<SystemAccount> SearchAccount(string searchNameAccount);
        Task<(int role, string message)> Login(AccountLogin accountLogin, HttpContext httpContext);
        Task<string> Logout(HttpContext httpContext);
        SystemAccount GetAccount(int id, out string message);
        string GetAccountName(int accountID, out string message);
    }

    public class SystemAccountRepository : ISystemAccountRepository
    {
        private string EMAIL_ADMIN;
        private string PASSWORD_ADMIN;
        private readonly FunewsManagementContext _context;
        private readonly IConfiguration _configuration;
        public SystemAccountRepository(FunewsManagementContext context, IConfiguration configuration, IHttpContextAccessor contextAccessor, AccountLogin _accountLogin)
        {
            _context = context;
            _configuration = configuration;
            EMAIL_ADMIN = _accountLogin.AccountEmail;
            PASSWORD_ADMIN = _accountLogin.Password;
        }

        public void CreateAccount(SystemAccount newAccount, out string message)
        {
            message = "";
            if (newAccount == null)
            {
                message = "Account is invalid!";
                return;
            }
            else
            {
                if (IsEmailExit(newAccount.AccountEmail) || newAccount.AccountEmail.Equals(EMAIL_ADMIN))
                {
                    message = "Email is exist!";
                    return;
                }
                _context.Add<SystemAccount>(newAccount);
                _context.SaveChanges();
            }
        }

        public void DeleteAccount(int id, out string message)
        {
            message = "";
            if (id == 0)
            {
                message = "AccountId is invalid!";
                return;
            }
            SystemAccount account = _context.SystemAccounts.FirstOrDefault(x => x.AccountId == id);
            if (account == null)
            {
                message = "Account is not exist!";
                return;
            }
            account.IsActive = false;
            _context.SystemAccounts.Update(account);
            _context.SaveChanges();
        }

        public List<SystemAccount> GetAccounts(out string message)
        {
            message = "";
            List<SystemAccount> accounts = _context.SystemAccounts.ToList();
            if (accounts.Count == 0)
            {
                message = "Danh sách tài khoản trống!";
                return accounts;
            }
            return accounts;
        }

        private bool IsEmailExit(string email)
        {
            if (email.IsNullOrEmpty()) return false;
            return _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(email)) != null;
        }

        public List<SystemAccount> SearchAccount(string searchNameAccount)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(int id, SystemAccount updateAccount, out string message)
        {
            message = "";
            var account = _context.SystemAccounts.FirstOrDefault(y => y.AccountId == id);
            if (account == null)
            {
                message = "Account is not exist!";
                return;
            }
            if (!account.AccountEmail.Equals(updateAccount.AccountEmail))
            {
                if (IsEmailExit(updateAccount.AccountEmail))
                {
                    message = "Email is exist!";
                    return;
                }
                account.AccountEmail = updateAccount.AccountEmail;
            }
            account.AccountName = updateAccount.AccountName;
            account.AccountRole = updateAccount.AccountRole;
            account.IsActive = updateAccount.IsActive;
            _context.SystemAccounts.Update(account);
            _context.SaveChanges();
        }

        public async Task<(int role, string message)> Login(AccountLogin accountLogin, HttpContext httpContext)
        {
            string message = "";
            if (accountLogin == null)
            {
                message = "Tài khoản không đúng!";
                return (0, message);
            }

            bool checkEmailAdmin = accountLogin.AccountEmail.Equals(EMAIL_ADMIN);
            if (!IsEmailExit(accountLogin.AccountEmail) && !checkEmailAdmin)
            {
                message = "Email không tồn tại!";
                return (0, message);
            }

            bool checkPassword = accountLogin.Password.Equals(PASSWORD_ADMIN);
            if (checkEmailAdmin && checkPassword)
            {
                // Nếu là Admin
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, EMAIL_ADMIN),
                    new Claim(ClaimTypes.Role, "3"),
                    new Claim("AccountID", "0") // Admin không có ID trong DB
                };
                await SignInUser(httpContext, claims);
                return (3, message);
            }
            else
            {
                var account = _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(accountLogin.AccountEmail) && x.AccountPassword.Equals(accountLogin.Password));
                if (account == null)
                {
                    message = "Mật khẩu không đúng!";
                    return (0, message);
                }
                else
                {
                    // Tạo Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, account.AccountEmail),
                        new Claim(ClaimTypes.Role, account.AccountRole.ToString()),
                        new Claim("AccountID", account.AccountId.ToString())
                    };

                    await SignInUser(httpContext, claims);
                    return (account.AccountRole, "");
                }
            }
        }
        private async Task SignInUser(HttpContext httpContext, List<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true }; // Cookie sẽ tồn tại sau khi đóng trình duyệt

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }


        public SystemAccount GetAccount(int id, out string message)
        {
            var account = new SystemAccount();
            message = "";
            if (id == 0)
            {
                message = "AccountID is invalid!";
                return account;
            }
            else
            {
                account = _context.SystemAccounts.FirstOrDefault(x => x.AccountId == id);
                if (account == null)
                {
                    message = "Account is not exits!";
                }
                return account;
            }
        }

        public string GetAccountName(int accountID, out string message)
        {
            message = "";
            var account = GetAccount(accountID, out message);
            if (!string.IsNullOrEmpty(message) || account == null)
            {
                return "";
            }
            return account.AccountName;
        }

        public async Task<string> Logout(HttpContext httpContext)
        {
            string message = "";
            try
            {
                // Đăng xuất người dùng
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Xóa toàn bộ claims trong HttpContext
                httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

                message = "Đăng xuất thành công!";
            }
            catch (Exception ex)
            {
                message = $"Lỗi khi đăng xuất: {ex.Message}";
            }
            return message;
        }


        private string GetRole(int role)
        {
            var roles = _configuration.GetSection("AccountRole").Get<Dictionary<string, int>>();

            if (roles != null)
            {
                foreach (var kvp in roles)
                {
                    if (kvp.Value == role)
                    {
                        return kvp.Key;
                    }
                }
            }

            return "Unknown"; // Trả về "Unknown" nếu không tìm thấy role
        }

    }
}
