/************************************************************************
 * Copyright(C) 2019 BlueStar. All rights reserved.
 * bluestar@hotmail.com
 * Created on 8/3/2019
************************************************************************/
using DBSetup.Interfaces;
using Models.data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSetup
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string clientRoleName = "client";
                const string investorRoleName = "investor";
                const string representanteRoleName = "representante";
                const string contactorRoleName = "contactor";
                const string servicemanagerRoleName = "servicemanager";
                const string debuggerdepartmentRoleName = "debuggerdepartment";
                const string collectiondepartmentRoleName = "collectiondepartment";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(clientRoleName, "Default agent", new string[] { });
                await EnsureRoleAsync(investorRoleName, "Default customer", new string[] { });
                await EnsureRoleAsync(representanteRoleName, "Default customer", new string[] { });
                await EnsureRoleAsync(contactorRoleName, "Default customer", new string[] { });
                await EnsureRoleAsync(servicemanagerRoleName, "Default customer", new string[] { });
                await EnsureRoleAsync(debuggerdepartmentRoleName, "Default customer", new string[] { });
                await EnsureRoleAsync(collectiondepartmentRoleName, "Default customer", new string[] { });

                await CreateUserAsync("admin", "tempP@ss123", "Default Administrator", "default_admin", "lucian@ancamakeup.com", "+1 (123) 000-0000","Super","Admin", new string[] { adminRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }
        }



        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string nickName, string email, string phoneNumber,string firstname,string lastname, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FirstName=firstname,
                LastName=lastname,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true,            
            };
            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);
            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            return applicationUser;
        }
    }
}
