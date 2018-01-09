using ActiveDs;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace AdPasswordReset
{
    class ADHelpers
    {
        public static IEnumerable<UserPrincipal> GetUsersInOu(string domain, string dn)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, dn);
            UserPrincipal queryUser = new UserPrincipal(context);
            PrincipalSearcher searcher = new PrincipalSearcher(queryUser);
            return searcher.FindAll().OrderBy(u => u.DisplayName.ToUpper()).Cast<UserPrincipal>();
        }

        public static void SetPassword(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username))
                {
                    user.SetPassword(password);
                }
            }
        }

        public static DateTime PasswordExpirationDate(UserPrincipal user)
        {
            DirectoryEntry entry = (DirectoryEntry)user.GetUnderlyingObject();
            IADsUser native = (IADsUser)entry.NativeObject;
            return native.PasswordExpirationDate;
        }
    }
}
