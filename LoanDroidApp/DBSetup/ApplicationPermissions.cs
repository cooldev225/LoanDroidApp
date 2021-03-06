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
        public static ApplicationPermission ViewUsers = new ApplicationPermission("Ver usuarias", "users.view", UsersPermissionGroupName, "Permission to view only own role users account details");
        public static ApplicationPermission ViewGlobalUsers = new ApplicationPermission("Ver usuarias de Grobal", "users.gview", UsersPermissionGroupName, "Permission to view all users account details");
        public static ApplicationPermission ManageUsers = new ApplicationPermission("Gestionar usuarias", "users.manage", UsersPermissionGroupName, "Permission to create, delete and modify other users account details");
        public static ApplicationPermission ViewClients = new ApplicationPermission("Ver cliente", "users.cview", UsersPermissionGroupName, "Permission to view only other clients account details");
        public static ApplicationPermission ManageClients = new ApplicationPermission("Gestionar clientes", "users.cmanage", UsersPermissionGroupName, "Permission to create, delete and modify other clients account details");
        public static ApplicationPermission ViewInvestors = new ApplicationPermission("Ver inversores", "users.iview", UsersPermissionGroupName, "Permission to view only other investors account details");
        public static ApplicationPermission ManageInvestors = new ApplicationPermission("Gestionar inversores", "users.imanage", UsersPermissionGroupName, "Permission to create, delete and modify other investors account details");

        public const string RolesPermissionGroupName = "Role Permissions";
        public static ApplicationPermission ViewRoles = new ApplicationPermission("Ver roles", "roles.view", RolesPermissionGroupName, "Permission to view available roles");
        public static ApplicationPermission ManageRoles = new ApplicationPermission("Administrar roles", "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");
        public static ApplicationPermission AssignRoles = new ApplicationPermission("Asignar roles", "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        public const string LoanPermissionGroupName = "Loan Permissions";
        public static ApplicationPermission RepresentanteLoan = new ApplicationPermission("Pedir representante", "loan.representante", LoanPermissionGroupName, "Permission to loan representante");
        public static ApplicationPermission RequestLoan = new ApplicationPermission("Pedir prestamo", "loan.request", LoanPermissionGroupName, "Permission to loan request");
        public static ApplicationPermission ServiceLoan = new ApplicationPermission("Pedir Servicio", "loan.service", LoanPermissionGroupName, "Permission to loan service");
        public static ApplicationPermission DebugLoan = new ApplicationPermission("Préstamo de depuracion", "loan.debug", LoanPermissionGroupName, "Permission to loan debug");
        public static ApplicationPermission CollectionLoan = new ApplicationPermission("Préstamo de cobranza", "loan.collection", LoanPermissionGroupName, "Permission to loan collection");

        public const string InvestmentPermissionGroupName = "Investment Permissions";
        public static ApplicationPermission RepresentanteInvestment = new ApplicationPermission("Inversión de representante", "investment.representante", InvestmentPermissionGroupName, "Permission to investment representante");
        public static ApplicationPermission RequestInvestment = new ApplicationPermission("Inversión de prestamo", "investment.request", InvestmentPermissionGroupName, "Permission to investment request");
        public static ApplicationPermission ServiceInvestment = new ApplicationPermission("Inversión de Servicio", "investment.service", InvestmentPermissionGroupName, "Permission to investment service");
        public static ApplicationPermission DebugInvestment = new ApplicationPermission("Inversión de depuracion", "investment.debug", InvestmentPermissionGroupName, "Permission to investment debug");
        public static ApplicationPermission CollectionInvestment = new ApplicationPermission("Inversión de cobranza", "investment.collection", InvestmentPermissionGroupName, "Permission to investment collection");
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

                RepresentanteLoan,
                RequestLoan,
                ServiceLoan,
                DebugLoan,
                CollectionLoan,

                RepresentanteInvestment,
                RequestInvestment,
                ServiceInvestment,
                DebugInvestment,
                CollectionInvestment
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
