using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardcodet.Wpf.Samples
{
    public static class IAUserInfo
    {
        public static int ID { get; set; }
        public static string UserName { get; set; }
        public static string ComputerName { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }
        public static string Bio { get; set; }
        public static bool RequireLogin { get; set; }
        public static byte[] Photo { get; set; }
        public static string Category1 { get; set; }
        public static string Category2 { get; set; }
        public static string Category3 { get; set; }
        public static string Category4 { get; set; }
        public static string Category5 { get; set; }
        public static string TextSearch { get; set; }
        public static string Category1Personal { get; set; }
        public static string Category2Personal { get; set; }
        public static string Category3Personal { get; set; }
        public static string Category4Personal { get; set; }
        public static string Category5Personal { get; set; }
        public static string TextSearchPersonal { get; set; }
        public static string Category1Public { get; set; }
        public static string Category2Public { get; set; }
        public static string Category3Public { get; set; }
        public static string Category4Public { get; set; }
        public static string Category5Public { get; set; }
        public static string TextSearchPublic { get; set; }
        public static DateTime? PurchaseDate { get; set; }
        public static string PurchaseDuration { get; set; }
    }
}
