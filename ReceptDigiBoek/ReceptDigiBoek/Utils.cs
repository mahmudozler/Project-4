using System;
using Xamarin.Forms;

namespace ReceptDigiBoek
{
	public class ProduceWindow
	{
		public static ContentPage Produce(string option)
		{
			switch (option)
			{
				default:
					return new Home();
				case "Home":
                    return new Home();
				case "Browse":
					throw new NotImplementedException();
				case "Login":
					throw new NotImplementedException();
				case "Register":
					throw new NotImplementedException();
			}
		}
	}
}