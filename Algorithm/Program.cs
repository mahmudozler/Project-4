using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
  public class Recipe
  {
    public string name;
    public string categorie;
    public string[] ingredients;
    public bool liked;

    public Recipe(string name, string categories, string ingredients, bool liked)
    {
      this.name = name;
      this.categorie = categories;
      this.ingredients = ingredients.Split(',');
      this.liked = liked;
    }
  }

  public static class DisplayRecipes
  {
    public static float Chance(Recipe recipe, List<Recipe> recipes)
    {
      float chance = 1;

      foreach (Recipe x in recipes)
      {
        if (x.name == recipe.name)
        {
          continue;
        }

        if (x.liked)
        {
          if (x.categorie == recipe.categorie)
          {
            chance = chance + 10;
          }

          foreach (string ingredient in recipe.ingredients)
          {
            foreach (string y in x.ingredients)
            {
              if (y == ingredient)
              {
                chance = chance + 2;
              }
            }
          }
        }
      }
      return chance;
    }

    public static void Display(List<Recipe> recipes, int count = 3)
    {
      //   Random random = new Random();
      List<int> recipeChance = new List<int>();
      int totalChance = 0;

      for (int x = 0; x < recipes.Count; x++)
      {
        totalChance = (int)(totalChance + DisplayRecipes.Chance(recipes[x], recipes));
      }

      for (int x = 0; x < recipes.Count; x++)
      {
        float chance = DisplayRecipes.Chance(recipes[x], recipes);
        float currentChance = chance / totalChance * 100.0f;
        for (int y = 0; y < currentChance; y++)
        {
          recipeChance.Add(x);
        }
      }

      Random random = new Random();
      List<int> randomRecipe = new List<int>();
      for (int x = 0; x < count; x++)
      {
        int randomInt = recipeChance[random.Next(0, totalChance)];
        if (randomRecipe.Contains(randomInt))
        {
          x = x - 1;
          continue;
        }
        randomRecipe.Add(randomInt);
        Console.WriteLine(recipes[randomRecipe[x]].name);
      }
    }
  }

  public class Program
  {
    static void Main()
    {
      Recipe recipe001 = new Recipe("Recipe 1", "Quick and Easy", "macaroni,chicken,butter,milk,cheese", true);
      Recipe recipe002 = new Recipe("Recipe 2", "Quick and Easy", "macaroni,broccoli,butter,milk,cheese", false);
      Recipe recipe003 = new Recipe("Recipe 3", "Asian", "rice,beef,soy sauce,salt", false);
      Recipe recipe004 = new Recipe("Recipe 4", "Asian", "noodles,pork,butter,milk,cheese", true);
      Recipe recipe005 = new Recipe("Recipe 5", "Italian", "spaghetti,meatballs,tomato sauce,cheese", true);
      Recipe recipe006 = new Recipe("Recipe 6", "Mexican", "tomato sauce,beef,beans,milk,cheese", false);

      List<Recipe> recipes = new List<Recipe> { recipe001, recipe002, recipe003, recipe004, recipe005, recipe006 };

      int totalChance = 0;

      foreach (Recipe x in recipes)
      {
        totalChance = (int)(totalChance + DisplayRecipes.Chance(x, recipes));
      }

      foreach (Recipe x in recipes)
      {
        Console.WriteLine("Chance of " + x.name + ": " + (DisplayRecipes.Chance(x, recipes)) / totalChance * 100.0f + "%");
      }

      Console.WriteLine("\nDISPLAY:");
      DisplayRecipes.Display(recipes);
    }
  }
}