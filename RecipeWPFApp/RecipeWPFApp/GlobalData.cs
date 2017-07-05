using System;

namespace RecipeWPFApp
{
    public static class Global
    {
        public static string status = "logged_out";
        public static string username { get; set; }
        public static string password { get; set; }
        public static int admin { get; set; }
    }
}