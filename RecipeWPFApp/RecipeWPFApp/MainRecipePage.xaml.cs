using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RecipeWPFApp
{
    /// <summary>
    /// Interaction logic for MainRecipePage.xaml
    /// </summary>
    public partial class MainRecipePage : Page
    {
        public MainRecipePage(Recipe recipe)
        {
            InitializeComponent();
            InitializeText(recipe);
        }

        private void InitializeText(Recipe recipe)
        {
            recipeName.Text = recipe.Title;
            recipeImage.Source = ToBitmapImage(recipe.Imagelink);
            recipeInstructions.Text = recipe.Bereidingswijze;

            var ingredientList = recipe.Ingredienten.Split(',').ToList();
            for (int x = 0; x < ingredientList.Count; x++)
            {
                if (ingredientList[x][0] == ' ')
                {
                    ingredientList[x] = ingredientList[x].Substring(1);
                }
                recipeIngredients.Children.Add(new TextBlock { Text = ingredientList[x], FontSize = 15 });
            }
        }

        private BitmapImage ToBitmapImage(string link)
        {
            BitmapImage source = new BitmapImage();

            source.BeginInit();
            source.UriSource = new Uri(link);
            source.EndInit();

            return source;
        }
    }
}
