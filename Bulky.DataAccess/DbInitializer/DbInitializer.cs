using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly ApplicationDbContext _db;
        private readonly ILogger<DbInitializer> _logger;
        public DbInitializer(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DbInitializer> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public void Initialize()
        {
            // migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database migration failed.");
            }
            // create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                // if role is not created, then create admin user as well
                const string adminEmail = "admin@gmail.com";
                var password = GenerateRandomPassword();

                var createResult = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "YaoChen WU",
                    PhoneNumber = "0912345678",
                    StreetAddress = "test 123 Ave",
                    State = "TW",
                    PostalCode = "500",
                    City = "Changhua"
                }, password).GetAwaiter().GetResult();

                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => $"{e.Code}:{e.Description}"));
                    _logger.LogError("Seed admin user failed. Email={Email}. Errors={Errors}", adminEmail, errors);
                    return;
                }

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == adminEmail);
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                _logger.LogWarning("Seeded admin account created. Email={Email} Password={Password}", adminEmail, password);

            }
            return;
        }

        private static string GenerateRandomPassword()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnopqrstuvwxyz";
            const string digits = "23456789";
            const string symbols = "!@$?_-#";

            var chars = new List<char>
            {
                Pick(upper),
                Pick(lower),
                Pick(digits),
                Pick(symbols)
            };

            const string all = upper + lower + digits + symbols;
            while (chars.Count < 16)
            {
                chars.Add(Pick(all));
            }

            Shuffle(chars);
            return new string(chars.ToArray());

            static char Pick(string s) => s[RandomNumberGenerator.GetInt32(s.Length)];
            static void Shuffle(IList<char> list)
            {
                for (int i = list.Count - 1; i > 0; i--)
                {
                    int j = RandomNumberGenerator.GetInt32(i + 1);
                    (list[i], list[j]) = (list[j], list[i]);
                }
            }
        }
    }
}
