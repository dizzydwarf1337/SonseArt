#nullable disable

using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace  SonseArt.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Profile => "Profile";

        public static string ChangePassword => "ChangePassword";

        public static string DeleteAccount => "DeleteAccount";

        public static string ExternalLogins => "ExternalLogins";

        public static string PersonalData => "PersonalData";

        public static string MyOrders => "MyOrders";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);


        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeleteAccount);


        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string MyOrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyOrders);
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
