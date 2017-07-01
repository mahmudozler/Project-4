using System;
namespace SQLform
{
    public class saveClass
    {
        public saveClass()
        {
        }
    }

    public static class info {
        //static string var1 = "this is var 1";
    }

    public static class Global
    {
        public static string status = "logged_out";
        public static string username { get; set; }
        public static string password { get; set; }
        public static int admin { get; set; }
    }
}
