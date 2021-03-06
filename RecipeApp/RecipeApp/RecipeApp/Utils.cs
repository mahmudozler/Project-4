﻿using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RecipeApp
{
    public class CreateWindow
        //instantiation logic is defined here (Factory design pattern)
    {
        public static ContentPage Create(int option)
        {
            switch (option)
            {
                case 1:
                    return new MainPage();
                case 2:
                    return new BrowsePage();
                case 3:
                    return new AccountPage();
				case 4:
					return new RegisterPage();
                default:
                    return new MainPage();
            }
        }
    }

	public interface Option<T>
        //Option interface which defines the signature for the visit method (part of visitor pattern)
	{
		U Visit<U>(Func<T, U> OnSome, Func<U> OnNone);
		void Visit(Action<T> OnSome, Action OnNone);
	}



    public interface Iterator<T> : System.Collections.Generic.IList<T>
        //Iterator interface which is part of the Iterator design pattern. Implements List<T> interface
	{
		Option<T> GetNext();
		void Reset();
		Option<T> GetCurrent();
	}

	public class Some<T> : Option<T>
        // Some<T> is used to determine whether a List contains an item (part of visitor pattern)
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
        //None<T> is used to determine whether a List contains no items (part of visitor pattern)
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
        // custom list which implements Iterator (part of Iterator pattern)
	{
        private System.Collections.Generic.List<T> Elements = new System.Collections.Generic.List<T>();
		public int Current = -1;

        public T this[int index] { get=>this.Elements[index]; set => this.Elements[index] = value; }

        public int Count => Elements.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            this.Elements.Add(item);
        }

        public void Clear()
        {
            this.Elements.Clear();
        }

        public bool Contains(T item)
        {
            return this.Elements.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Elements.CopyTo(array, arrayIndex);
        }

        public Option<T> GetCurrent()
		{
			try
			{
				return new Some<T>(Elements[Current]);
			}
			catch (ArgumentOutOfRangeException) { return new None<T>(); }
		}

        public IEnumerator<T> GetEnumerator()
        {
            return this.Elements.GetEnumerator();
        }

        public Option<T> GetNext()
		{
			this.Current += 1;
			return GetCurrent();
		}

        public int IndexOf(T item)
        {
            return this.Elements.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.Elements.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return this.Elements.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.Elements.RemoveAt(index);
        }

        public void Reset()
		{
			this.Current = -1;
		}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Elements.GetEnumerator();
        }
    }

    public class Recipe
        //recipe class contains all data of one recipe
    {
		public string ID { get; set; }
		public string Title { get; set; }
		public object Beschrijving { get; set; }
		public string Ingredienten { get; set; }
		public object Voorbereiding { get; set; }
		public string Bereidingswijze { get; set; }
		public string Categorie { get; set; }
		public string Imagelink { get; set; }
		public object Valid { get; set; }
    }

	public class User
        //user class contains all data of one user
	{
		public string username { get; set; }
		public string password { get; set; }
		public string admin { get; set; }
	}

	public class RegisterResponse
        //RegisterResponse class is used to determine whether an account registration was successfull
	{
		public bool status { get; set; }
	}

	public class BookmarkItem
        //Contains a bookmark made by an user
	{
		public string username { get; set; }
        public string recept { get; set; }
	}
	public class Average
        //Contains average rating of a recipe
	{
		public string beoordeling { get; set; }
	}
	public class UserRating
        //Contains rating of an user on a recipe
	{
		public string beoordeling { get; set; }
        public string recept { get; set; }
        public string username { get; set; }
	}
}