using System;
using Xamarin.Forms;

namespace RecipeApp
{
    public class CreateWindow
    {
        public static ContentPage Create(int option)
        {
            switch (option)
            {
                case 1:
                    return new MainPage();
                case 2:
                    throw new NotImplementedException();
                case 3:
                    throw new NotImplementedException();
                default:
                    return new MainPage();
            }
        }

		public interface Option<T>
		{
			U Visit<U>(Func<T, U> OnSome, Func<U> OnNone);
			void Visit(Action<T> OnSome, Action OnNone);
		}

		public interface Iterator<T>
		{
			Option<T> GetNext();
			void Reset();
			Option<T> GetCurrent();
		}

		public class Some<T> : Option<T>
		{
			private T Value;

			public Some(T item)
			{
				this.Value = item;
			}

			public U Visit<U>(Func<T, U> OnSome, Func<U> OnNone)
			{
				return OnSome(this.Value);
			}

			public void Visit(Action<T> OnSome, Action OnNone)
			{
				OnSome(this.Value);
			}
		}

		public class None<T> : Option<T>
		{
			public U Visit<U>(Func<T, U> OnSome, Func<U> OnNone)
			{
				return OnNone();
			}

			public void Visit(Action<T> OnSome, Action OnNone)
			{
				OnNone();
			}
		}

		public class List<T> : Iterator<T>
		{
			public System.Collections.Generic.List<T> Elements = new System.Collections.Generic.List<T>();
			public int Current = -1;

			public void Add(T value)
			{
				Elements.Add(value);
			}

			public Option<T> GetCurrent()
			{
				try
				{
					return new Some<T>(Elements[Current]);
				}
				catch (ArgumentOutOfRangeException) { return new None<T>(); }
			}

			public Option<T> GetNext()
			{
				this.Current += 1;
				return GetCurrent();
			}

			public void Reset()
			{
				this.Current = -1;
			}
		}

        public class Recipe
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> Ingredients { get; set; }

            public Recipe(int index)
            {
                
            }

            public Option<Recipe> Filter(string str)
            {
                if (Ingredients.Elements.Contains(str))
                {
                    return new Some<Recipe>(this);
                }
                else return new None<Recipe>();
            }
        }

    }
}