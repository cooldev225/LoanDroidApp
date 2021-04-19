// =============================
// Email: bluestar1027@hotmail.com
// =============================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace DBSetup{
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;
        public const string UsersPermissionGroupName = "User Permissions";
        public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view", UsersPermissionGroupName, "Permission to view only own role users account details");
        public static ApplicationPermission ViewGlobalUsers = new ApplicationPermission("View Grobal Users", "users.gview", UsersPermissionGroupName, "Permission to view all users account details");
        public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");
        public static ApplicationPermission ViewClients = new ApplicationPermission("View Client", "users.cview", UsersPermissionGroupName, "Permission to view only other clients account details");
        public static ApplicationPermission ManageClients = new ApplicationPermission("Manage Clients", "users.cmanage", UsersPermissionGroupName, "Permission to create, delete and modify other clients account details");
        public static ApplicationPermission ViewInvestors = new ApplicationPermission("View Investors", "users.iview", UsersPermissionGroupName, "Permission to view only other investors account details");
        public static ApplicationPermission ManageInvestors = new ApplicationPermission("Manage Investors", "users.imanage", UsersPermissionGroupName, "Permission to create, delete and modify other investors account details");

        public const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        public const string LoanPermissionGroupName = "Loan Permissions";
        public static ApplicationPermission RequestLoan = new ApplicationPermission("Request Loan", "loan.request", LoanPermissionGroupName, "Permission to loan request");
        public static ApplicationPermission DebugLoan = new ApplicationPermission("Debug Loan", "loan.debug", LoanPermissionGroupName, "Permission to loan debug");
        public static ApplicationPermission CollectionLoan = new ApplicationPermission("Collection Loan", "loan.collection", LoanPermissionGroupName, "Permission to loan collection");
        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
                ViewUsers,
                ViewGlobalUsers,
                ManageUsers,
                ViewClients,
                ManageClients,
                ViewInvestors,
                ManageInvestors,

                ViewRoles,
                ManageRoles,
                AssignRoles,

                RequestLoan,
                DebugLoan,
                CollectionLoan
            };
            AllPermissions = allPermissions.AsReadOnly();
        }
        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).SingleOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).SingleOrDefault();
        }
        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }
        public static string[] GetAdministrativePermissionValues()
        {
            return new string[] { ManageUsers, ManageRoles, AssignRoles };
        }
    }
    public class ApplicationPermission
    {
        public ApplicationPermission()
        { }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Value;
        }
        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}
