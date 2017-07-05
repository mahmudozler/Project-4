using System;
namespace RecipeApp
{
	public static class Global
        //Data which is available without instantiation
	{
		public static string status = "logged_out";
		public static string username { get; set; }
		public static string password { get; set; }
		public static int admin { get; set; }
	}
}
