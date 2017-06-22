using System;
using Xamarin.Forms;

namespace RecipeApp
{
    public class CreateWindow
    {
        public static ContentPage Create(int option)
        {
            switch(option)
            {
                default:
                    return new MainPage();
                case 1:
                    return new MainPage();
				case 2:
                    throw new NotImplementedException();
				case 3:
                    throw new NotImplementedException();

			}
        }
    }
}