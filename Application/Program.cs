﻿using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application
{
	public static class Program
	{
		static Program()
		{
		}

		private static Persistence.DatabaseContext _databaseContext;

		/// <summary>
		/// Lazy Loading = Lazy Initialization
		/// </summary>
		public static Persistence.DatabaseContext DatabaseContext
		{
			get
			{
				if (_databaseContext == null)
				{
					_databaseContext =
						new Persistence.DatabaseContext();
				}

				return _databaseContext;
			}
		}

		public static void Main()
		{
			try
			{
				// **************************************************
				{
					// دو دستور ذيل کاملا با هم معادل می باشند

					DatabaseContext.Countries
						.Load();

					// استفاده می کنيم DatabaseContext.Countries.Local.ToObservableCollection() از

					var countries =
						DatabaseContext.Countries
						.ToList()
						;

					// countries معادل DatabaseContext.Countries.Local.ToObservableCollection()

					// "SELECT * FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10"
				}
				// **************************************************

				// **************************************************
				// دو دستور ذيل با هم معادل می‌باشند
				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}

				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10 && current.Code <= 20)
						.ToList()
						;

				}

				// "SELECT * FROM Countries WHERE Code >= 10 AND Code <= 20"
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code < 10 || current.Code > 20)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code < 10 OR Code > 20"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name == "Iran")
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name = 'Iran'"
				}
				// **************************************************

				// **************************************************
				// true -> IgnoreCase
				// کار نمی‌کند EF Core دستور ذیل در
				//{
				//	var countries =
				//		DatabaseContext.Countries
				//		.Where(current => string.Compare(current.Name, "Iran", true) == 0)
				//		.ToList()
				//		;
				//}
				// "SELECT * FROM Countries WHERE Name = 'Iran'"
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToLower() == "Iran".ToLower())
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name = 'Iran'"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToUpper() == "Iran".ToUpper())
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name = 'Iran'"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToUpper().StartsWith("Iran".ToLower()))
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name LIKE 'Iran%'"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToUpper().EndsWith("Iran".ToLower()))
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name LIKE '%Iran'"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToUpper().Contains("Iran".ToLower()))
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Name LIKE '%Iran%'"
				}
				// **************************************************

				// **************************************************
				// Note: دقت کنيد که دستور ذيل کار نمی کند
				// **************************************************
				{
					string text = "Ali Alavi";

					text =
						text.Replace(" ", "%"); // "Ali%Alavi"

					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToLower().Contains(text.ToLower()))
						.ToList()
						;

					// EF, EF Core -> SQL Injection Free

					// "SELECT * FROM Countries WHERE Name LIKE '%Ali Alavi%'"
					// "SELECT * FROM Countries WHERE Name LIKE '%Ali%Alavi%'" -> "Seyed Ali Reza Alavi Asl"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Name.ToLower().Contains("Ali".ToLower()))
						.Where(current => current.Name.ToLower().Contains("Alavi".ToLower()))
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current =>
							current.Name.ToLower().Contains("Ali".ToLower()) ||
							current.Name.ToLower().Contains("Alavi".ToLower()))
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.OrderBy(current => current.Code)
						.ToList()
						;

					// "SELECT * FROM Countries ORDER BY Code"
					// "SELECT * FROM Countries ORDER BY Code ASC"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.OrderByDescending(current => current.Code)
						.ToList()
						;

					// "SELECT * FROM Countries ORDER BY Code DESC"
				}
				// **************************************************

				// **************************************************
				// خطا می‌دهد EF دستور ذیل در
				{
					var countries =
						DatabaseContext.Countries
						.OrderBy(current => current.Code)
						.Where(current => current.Code >= 10)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code DESC"
				}
				// **************************************************

				// **************************************************
				// عادت نکنید که از روش (دستور) فوق استفاده کنید
				// بهتر است از روش ذیل استفاده کنید
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderBy(current => current.Code)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code DESC"
				}
				// **************************************************

				// **************************************************
				// Note: Wrong Usage!
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderBy(current => current.Code)
						.OrderBy(current => current.Name)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code, Name"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderBy(current => current.Code)
						.ThenBy(current => current.Name)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code, Name"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderBy(current => current.Code)
						.ThenByDescending(current => current.Name)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code, Name DESC"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderByDescending(current => current.Code)
						.ThenBy(current => current.Name)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code DESC, Name"
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Where(current => current.Code >= 10)
						.OrderByDescending(current => current.Code)
						.ThenByDescending(current => current.Name)
						.ToList()
						;

					// "SELECT * FROM Countries WHERE Code >= 10 ORDER BY Code DESC, Name DESC"
				}
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					var country =
						DatabaseContext.Countries
						.Where(current => current.Code == 1)
						.FirstOrDefault();

					// With Lazy Loading	With Include	-> OK
					// Without Lazy Loading	With Include	-> OK

					// With Lazy Loading,	Without Include	-> OK
					// Without Lazy Loading	Without Include	-> Error!
					int stateCount =
						country.States.Count;
				}
				// **************************************************

				// **************************************************
				{
					var states =
						DatabaseContext.States
						.Where(current => current.Country.Code == 1)
						.ToList()
						;

					int stateCount = states.Count;
				}
				// **************************************************

				// **************************************************
				{
					int stateCount =
						DatabaseContext.States
						.Where(current => current.Country.Code == 1)
						.Count();
				}
				// **************************************************

				// **************************************************
				{
					var country =
						DatabaseContext.Countries
						.Where(current => current.Code == 10)
						.FirstOrDefault();

					// Explicit Loading
					int stateCount =
						DatabaseContext.Entry(country)
							.Collection(current => current.States)
							.Query()
							.Count();
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					var country =
						DatabaseContext.Countries
						.Where(current => current.Code == 10)
						.FirstOrDefault();

					// فاجعه
					var states =
						country.States
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var states =
						DatabaseContext.States
						.Where(current => current.Country.Code == 10)
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var country =
						DatabaseContext.Countries
						.Where(current => current.Code == 10)
						.FirstOrDefault();

					var states =
						DatabaseContext.Entry(country)
						.Collection(current => current.States)
						.Query()
						.Where(state => state.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				// اضافه شده است EF Core دستور ذیل در
				{
					var country =
						DatabaseContext.Countries
						.Include(current => current.States.Where(state => state.Code <= 20))
						.Where(current => current.Code == 10)
						.FirstOrDefault();

					var states =
						country.States;
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Include(navigationPropertyPath: "States")
						.Where(current => current.Code <= 10)
						.ToList()
						;

					// در این حالت امکان بی‌دقتی وجود دارد

					// Runtime Error!
					//var countries =
					//	DatabaseContext.Countries
					//	.Include(navigationPropertyPath: "Statse")
					//	.Where(current => current.Code <= 10)
					//	.ToList()
					//	;
				}
				// **************************************************

				// **************************************************
				{
					// Note: Strongly Typed
					var countries =
						DatabaseContext.Countries
						.Include(current => current.States)
						.Where(current => current.Code <= 10)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						.Include(navigationPropertyPath: "States")
						.Include(navigationPropertyPath: "States.Cities")
						.Where(current => current.Code <= 10)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					// Note: Strongly Typed
					var countries =
						DatabaseContext.Countries
						.Include(current => current.States)
						.Include(current => current.States.Select(state => state.Cities))
						.Where(current => current.Code <= 10)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				//{
				//	// Note: Strongly Typed
				//	var countries =
				//		DatabaseContext.Countries
				//		.Include(current => current.States)
				//		.Include(current => current.States.Select(state => state.Cities))
				//		.Include(current => current.States.Select(state => state.Cities.Select(city => city.Sections)))
				//		.Where(current => current.Code <= 10)
				//		.ToList()
				//		;
				//}
				// **************************************************

				// **************************************************
				{
					var cities =
						DatabaseContext.Cities
						.Include(navigationPropertyPath: "State")
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var cities =
						DatabaseContext.Cities
						.Include(current => current.State)
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var cities =
						DatabaseContext.Cities
						.Include(navigationPropertyPath: "State")
						.Include(navigationPropertyPath: "State.Country")
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var cities =
						DatabaseContext.Cities
						.Include(current => current.State)
						.Include(current => current.State.Country)
						.Where(current => current.Code <= 20)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				// صورت مساله
				// تمام کشورهايی را می‌خواهیم که لااقل در نام يکی از استان‌های آن حرف {بی} وجود داشته باشد
				// لااقل = Any
				// **************************************************

				// **************************************************
				{
					// Country						State
					// ----------------------------------
					// Country1						State1
					// Country2						State2(B)
					// Country2						State3(B)
					// Country2						State4
					// Country3						State5(B)
					// Country3						State6
					// Country4						State7
					// Country4						State8

					// (1)
					var countries =
						DatabaseContext.Countries
						// دقت کنيد که صرف شرط ذيل، نيازی به دستور
						// Include
						// نيست
						//.Include(current => current.States)
						.Where(current => current.States.Any(state => state.Name.ToLower().Contains("B".ToLower())))
						.ToList()
						;

					// Country2, Country3

					// (2)
					// Note: Wrong Answer
					//var countries =
					//	DatabaseContext.States
					//	.Where(current => current.Name.ToLower().Contains("B".ToLower()))
					//	.Select(current => current.Country)
					//	.ToList()
					//	;

					// Country2, Country2, Country3
				}
				// **************************************************

				// **************************************************
				// صورت مساله
				// تمام کشورهايی را می‌خواهیم که در لااقل نام يکی از شهرهای آن حرف {بی} وجود داشته باشد
				// **************************************************

				// **************************************************
				{
					var countries =
						DatabaseContext.Countries
						// دقت کنيد که صرف شرط ذيل، نيازی به دستور
						// Include
						// نيست
						//.Include(current => current.States)
						//.Include(current => current.States.Select(state => state.Cities))
						.Where(current => current.States.Any(state => state.Cities.Any(city => city.Name.Contains("B"))))
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				// صورت مساله
				// تمام شهرهايی را می‌خواهيم که جمعيت کشور آنها بيش از يکصد ميليون نفر باشد
				// **************************************************

				{
					var cities =
						DatabaseContext.Cities
						// دقت کنيد که صرف شرط ذيل، نيازی به دستور
						// Include
						// نيست
						//.Include(current => current.State)
						//.Include(current => current.State.Country)
						.Where(current => current.State.Country.Population >= 100)
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				// Country -> State -> City -> Region -> Hotel
				// **************************************************

				// **************************************************
				//{
				//	var hotels =
				//		DatabaseContext.Hotels
				//		.ToList()
				//		;
				//}

				//{
				//	var hotels =
				//		DatabaseContext.Hotels
				//		.Where(current => current.RegionId == viewModel.regionId)
				//		.ToList()
				//		;
				//}

				//{
				//	var hotels =
				//		DatabaseContext.Hotels
				//		.Where(current => current.Region.CityId == viewModel.cityId)
				//		.ToList()
				//		;
				//}

				//{
				//	var hotels =
				//		DatabaseContext.Hotels
				//		.Where(current => current.Region.City.StateId == viewModel.stateId)
				//		.ToList()
				//		;
				//}

				//{
				//	var hotels =
				//		DatabaseContext.Hotels
				//		.Where(current => current.Region.City.State.CountryId == viewModel.countryId)
				//		.ToList()
				//		;
				//}
				// **************************************************
				// **************************************************
				// **************************************************










				// **************************************************
				int countryCount = 0;
				// **************************************************

				// **************************************************
				// خاطره
				//DatabaseContext.Countries
				//	.Where(current => current.Code => 5)
				//	.Where(current => current.Code <= 45)
				//	.Load();
				// **************************************************

				// **************************************************
				DatabaseContext.Countries
					.Where(current => current.Code >= 5)
					.Where(current => current.Code <= 45)
					.Load();

				countryCount =
					DatabaseContext.Countries.Local.Count;

				DatabaseContext.Countries
					.Where(current => current.Code >= 10)
					.Where(current => current.Code <= 50)
					.Load();

				countryCount =
					DatabaseContext.Countries.Local.Count;
				// **************************************************

				string countryNameString = string.Empty;
				string countryCodeToString = string.Empty;
				string countryCodeFromString = string.Empty;

				// **************************************************
				// **************************************************
				// **************************************************
				//var data =
				//	DatabaseContext.Countries
				//		.Where(current => current.IsActive)
				//		;

				//var data =
				//	DatabaseContext.Countries
				//		.Where(current => 1 == 1)
				//		;

				var data =
					DatabaseContext.Countries
					.AsQueryable()
					;

				if (string.IsNullOrWhiteSpace(countryNameString) == false)
				{
					data =
						data
						.Where(current => current.Name.ToLower().Contains(countryNameString.ToLower()))
						;
				}

				if (string.IsNullOrWhiteSpace(countryCodeFromString) == false)
				{
					// Note: Wrong Usage!
					//data =
					//	data
					//	.Where(current => current.Code >= System.Convert.ToInt32(countryCodeFromString))
					//	;

					int countryCodeFrom =
						System.Convert.ToInt32(countryCodeFromString);

					data =
						data
						.Where(current => current.Code >= countryCodeFrom)
						;
				}

				if (string.IsNullOrWhiteSpace(countryCodeToString) == false)
				{
					int countryCodeTo =
						System.Convert.ToInt32(countryCodeToString);

					data =
						data
						.Where(current => current.Code <= countryCodeTo)
						;
				}

				data =
					data
					.OrderBy(current => current.Id)
					;

				data
					.Load();

				// يا

				// Note: Wrong Usage!
				//data = data.ToList();

				var result =
					data.ToList()
					;

				// result معادل DatabaseContext.Countries.Local
				// **************************************************

				// **************************************************
				string search1 = "   Ali       Reza  Iran Carpet   Ali         ";

				string[] keywords1 =
					{ "Ali", "Reza", "Iran", "Carpet" }; // یه جوری

				var dataTemp1 =
					DatabaseContext.Countries
					.AsQueryable();

				foreach (var keyword in keywords1)
				{
					dataTemp1 =
						dataTemp1
						.Where(current => current.Name.Contains(keyword))
						;
				}

				dataTemp1 =
					dataTemp1
					.OrderBy(current => current.Code)
					;

				var dataResult1 =
					dataTemp1
					.ToList()
					;
				// **************************************************

				// **************************************************
				string search2 = "   Ali       Reza  Iran Carpet   Ali         ";

				search2 = search2.Trim();

				//search2 = "Ali       Reza  Iran Carpet   Ali";

				while (search2.Contains("  "))
				{
					search2 =
						search2.Replace("  ", " ");
				}

				//search2 = "Ali Reza Iran Carpet Ali";

				//string[] keywords =
				//	search2.Split(' ');

				//keywords = { "Ali", "Reza", "Iran", "Carpet", "Ali" }

				var keywords =
					search2.Split(' ').Distinct<string>();

				//keywords = { "Ali", "Reza", "Iran", "Carpet" };

				var dataTemp =
					DatabaseContext.Countries
					.AsQueryable()
					;

				// Solution (1)
				foreach (string keyword in keywords)
				{
					dataTemp =
						dataTemp
						.Where(current => current.Name.ToLower().Contains(keyword.ToLower()))
						;
				}
				// /Solution (1)

				// Solution (2)
				// Mr. Farshad Rabiei
				// دستور ذیل باید چک شود
				//dataTemp =
				//	dataTemp.Where(current => keywords.Contains(current.Name));
				// /Solution (2)

				dataTemp =
					dataTemp
					.OrderBy(current => current.Code)
					;

				dataTemp
					.Load();

				// يا

				var dataResult =
					dataTemp
					.ToList()
					;
				// **************************************************

				// **************************************************
				// روش اول
				// دستورات ذيل کاملا با هم معادل هستند
				DatabaseContext.Countries
					.Load();

				// DatabaseContext.Countries.Local

				// روش دوم
				var someData0100 =
					DatabaseContext.Countries
					.ToList()
					;

				// روش سوم
				var someData0200 =
					from Country in DatabaseContext.Countries
					select Country
					;

				// SELECT * FROM Countries
				// **************************************************

				// **************************************************
				// ها Country آرایه ای از
				var someData0300 =
					from Country in DatabaseContext.Countries
					where Country.Name.ToLower().Contains("Iran".ToLower())
					select Country
					;
				// **************************************************

				// **************************************************
				// ها Country آرایه ای از
				var someData0400 =
					from Country in DatabaseContext.Countries
					where Country.Name.ToLower().Contains("Iran".ToLower())
					orderby Country.Name
					select Country
					;

				foreach (Domain.Country currentCountry in someData0400)
				{
					System.Console.WriteLine(currentCountry.Name);
				}
				// **************************************************

				// **************************************************
				// ها String آرایه ای از
				// (A)
				var someData0500 =
					from Country in DatabaseContext.Countries
					where Country.Name.ToLower().Contains("Iran".ToLower())
					orderby Country.Name
					select Country.Name
					;

				// Select Name From Countries

				// Note: Wrong Usage!
				//foreach (Models.Country currentCountry in someData0500)
				//{
				//	System.Console.WriteLine(currentCountry.Name);
				//}

				foreach (string currentCountryName in someData0500)
				{
					System.Console.WriteLine(currentCountryName);
				}
				// **************************************************

				// **************************************************
				// Note: See Learning Anonymous Object File!
				// **************************************************

				// **************************************************
				// ها Object آرایه ای از
				// (B)
				var someData0600 =
					from Country in DatabaseContext.Countries
					where Country.Name.ToLower().Contains("Iran".ToLower())
					orderby Country.Name
					select new { Name = Country.Name }
					;

				// Note: Wrong Usage!
				//foreach (Models.Country currentCountry in someData0600)
				//{
				//	System.Windows.Forms.MessageBox.Show(currentCountry.Name);
				//}

				foreach (var currentPartialCountry in someData0600)
				{
					System.Console.WriteLine
						(currentPartialCountry.Name);
				}
				// **************************************************

				// **************************************************
				var someData0700 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select new { Country.Name }
					;

				foreach (var currentPartialCountry in someData0700)
				{
					System.Console.WriteLine(currentPartialCountry.Name);
				}
				// **************************************************

				// **************************************************
				// (C)
				var someData0800 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select new { Baghali = Country.Name }
					;

				// Note: Wrong Usage!
				//foreach (Models.Country currentCountry in someData0800)
				//{
				//	System.Windows.Forms.MessageBox.Show(currentCountry.Name);
				//}

				// Note: Wrong Usage!
				//foreach (var currentPartialCountry in someData0800)
				//{
				//	System.Windows.Forms.MessageBox.Show(currentPartialCountry.Name);
				//}

				foreach (var currentPartialCountry in someData0800)
				{
					System.Console.WriteLine
						(currentPartialCountry.Baghali);
				}
				// **************************************************

				// **************************************************
				var someData0900 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select new { Size = Country.Population, Country.Name }
					;

				foreach (var currentPartialCountry in someData0900)
				{
					System.Console.WriteLine
						(currentPartialCountry.Name);
				}
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				// (D1)
				var someData1001 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select (new CountryViewModel1() { NewName = Country.Name })
					;

				foreach (CountryViewModel1 currentCountryViewModel in someData1001)
				{
					currentCountryViewModel.NewName += "!";

					System.Console.WriteLine
						(currentCountryViewModel.NewName);
				}
				// **************************************************

				// **************************************************
				// (D1)
				var someData1002 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select (new CountryViewModel2() { Name = Country.Name })
					;

				foreach (CountryViewModel2 currentCountryViewModel in someData1002)
				{
					currentCountryViewModel.Name += "!";

					System.Console.WriteLine
						(currentCountryViewModel.Name);
				}
				// **************************************************

				// **************************************************
				// (D3)
				// Note: Wrong Usage!
				//var someData1003 =
				//	from Country in DatabaseContext.Countries
				//	where Country.Name.Contains("Iran")
				//	orderby Country.Name
				//	select (new CountryViewModel2() { Country.Name })
				//	;

				//foreach (CountryViewModel2 currentCountryViewModel in someData1003)
				//{
				//	currentCountryViewModel.Name += "!";

				//	System.Windows.Forms.MessageBox.Show(currentCountryViewModel.Name);
				//}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// (E)
				// Note: متاسفانه کار نمی کند
				var someData1100 =
					from Country in DatabaseContext.Countries
					where Country.Name.Contains("Iran")
					orderby Country.Name
					select new Domain.Country() { Name = Country.Name }
					;

				foreach (Domain.Country currentCountry in someData1100)
				{
					System.Console.WriteLine(currentCountry.Name);
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				var someData1200 =
					DatabaseContext.Countries
					.ToList()
					;

				// "SELECT * FROM Countries"
				// **************************************************

				// **************************************************
				// It is similar to (A)
				var someData1300 =
					DatabaseContext.Countries
					.Select(current => current.Name)
					.ToList()
					;

				// "SELECT Name FROM Countries"
				// **************************************************

				// **************************************************
				// It is similar to (B)
				var someData1400 =
					DatabaseContext.Countries
					.Select(current => new { Name = current.Name })
					.ToList()
					;

				// "SELECT Name FROM Countries"
				// **************************************************

				// **************************************************
				// It is similar to (B)
				var someData1500 =
					DatabaseContext.Countries
					.Select(current => new { current.Name })
					.ToList()
					;

				// "SELECT Name FROM Countries"
				// **************************************************

				// **************************************************
				// It is similar to (C)
				var someData1600 =
					DatabaseContext.Countries
					.Select(current => new { Baghali = current.Name })
					.ToList()
					;

				// "SELECT Name FROM Countries"
				// **************************************************

				// **************************************************
				// It is similar to (D)
				var someData17001 =
					DatabaseContext.Countries
					.Select(current => new CountryViewModel1() { NewName = current.Name })
					.ToList()
					;
				// **************************************************

				// **************************************************
				// It is similar to (D)
				var someData17002 =
					DatabaseContext.Countries
					.Select(current => new CountryViewModel2() { Name = current.Name })
					.ToList()
					;
				// **************************************************

				// **************************************************
				// Note: Wrong Usage!
				// It is similar to (D)
				//var someData17003 =
				//	DatabaseContext.Countries
				//	.Select(current => new CountryViewModel2() { current.Name })
				//	.ToList()
				//	;
				// **************************************************

				// **************************************************
				// It is similar to (E)
				// Note: متاسفانه کار نمی کند
				var someData1800 =
					DatabaseContext.Countries
					.Select(current => new Domain.Country() { Name = current.Name })
					.ToList()
					;
				// **************************************************

				// **************************************************
				var someData1900 =
					DatabaseContext.Countries
					.Select(current => new { current.Name })
					.ToList()
					.Select(current => new Domain.Country()
					{
						Name = current.Name,
					})
					.ToList()
					;

				// "SELECT Name FROM Countries"
				// **************************************************

				// **************************************************
				var someData2000 =
					DatabaseContext.Countries
					.Select(current => new { current.Id, current.Name })
					.ToList()
					.Select(current => new Domain.Country()
					{
						Id = current.Id,
						Name = current.Name,
					})
					.ToList()
					;

				// "SELECT Id, Name FROM Countries"
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				var someData2100 =
					DatabaseContext.Countries
					.Select(current => new
					{
						Id = current.Id,
						Name = current.Name,
						StateCount = current.States.Count,
					})
					.ToList()
					;
				// **************************************************

				// **************************************************
				var someData2200 =
					DatabaseContext.Countries
					.Select(current => new
					{
						current.Id,
						current.Name,
						StateCount = current.States.Count,
					})
					.ToList()
					;
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				// در دو دستور ذیل در صورتی که تحت شرایطی تعداد استان‌ها برای یک کشور صفر باشد، خطا ایجاد می‌شود
				var someData2300 =
					DatabaseContext.Countries
					.Select(current => new
					{
						current.Id,
						current.Name,
						StateCount = current.States.Count,
						CityCount = current.States.Sum(state => state.Cities.Count),
					})
					.ToList()
					;

				var someData2400 =
					DatabaseContext.Countries
					.Select(current => new
					{
						current.Id,
						current.Name,
						StateCount = current.States.Count,
						CityCount = current.States.Select(state => state.Cities.Count).Sum(),
					})
					.ToList()
					;

				var someData2500 =
					DatabaseContext.Countries
					.Select(current => new
					{
						current.Id,
						current.Name,

						StateCount = current.States.Count,

						CityCount = current.States.Count == 0 ? 0 :
							current.States.Select(state => new { XCount = state.Cities.Count }).Sum(x => x.XCount)

						//CityCount = current.States.Count == 0 ? 0 :
						//	current.States.Select(state => state.Cities.Count).Sum()

						//CityCount = current.States == null || current.States.Count == 0 ? 0 :
						//	current.States.Select(state => new { XCount = state.Cities == null ? 0 : state.Cities.Count }).Sum(x => x.XCount)
					})
					.ToList()
					;

				// مهدی اکبری
				var someData2600 =
					DatabaseContext.Countries
					.Select(current => new
					{
						current.Id,
						current.Name,

						StateCount = current.States.Count,

						CityCount = current.States.Select(state => state.Cities.Count).DefaultIfEmpty(0).Sum(),
					})
					.ToList()
					;
				// **************************************************
				// **************************************************
				// **************************************************

				// Group By

				var someData2700 =
					DatabaseContext.Countries
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.ToList()
					;

				var someData2800 =
					DatabaseContext.Countries
					.Where(current => current.Population >= 120000000)
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.ToList()
					;

				var someData2900 =
					DatabaseContext.Countries
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.Where(ao => ao.Population >= 120000000)
					.ToList()
					;

				var someData3000 =
					DatabaseContext.Countries
					.Where(current => current.Name.Contains('I'))
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.ToList()
					;

				// Note: Wrong Usage!
				//var someData3100 =
				//	DatabaseContext.Countries
				//	.GroupBy(current => current.Population)
				//	.Select(current => new
				//	{
				//		Population = current.Key,

				//		Count = current.Count(),
				//	})
				//	.Where(ao => ao.Name.Contains('ا'))
				//	.ToList()
				//	;

				var someData3200 =
					DatabaseContext.Countries
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.Where(ao => ao.Count >= 30)
					.ToList()
					;

				var someData3300 =
					DatabaseContext.Countries
					.Where(current => current.Name.Contains('I'))
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),
					})
					.Where(ao => ao.Count >= 30)
					.ToList()
					;

				var someData3400 =
					DatabaseContext.Countries
					.GroupBy(current => new { current.Population, current.HealthyRate })
					.Select(current => new
					{
						Population = current.Key.Population,
						HealthyRate = current.Key.HealthyRate,

						Count = current.Count(),
					})
					.ToList()
					;

				var someData3500 =
					DatabaseContext.Countries
					.GroupBy(current => new { current.Population, current.HealthyRate })
					.Select(current => new
					{
						current.Key.Population,
						current.Key.HealthyRate,

						Count = current.Count(),
					})
					.ToList()
					;

				var someData3600 =
					DatabaseContext.Countries
					.GroupBy(current => current.Population)
					.Select(current => new
					{
						Population = current.Key,

						Count = current.Count(),

						Max = current.Max(x => x.HealthyRate),
						Min = current.Min(x => x.HealthyRate),
						Sum = current.Sum(x => x.HealthyRate),
						Average = current.Average(x => x.HealthyRate),
					})
					.ToList()
					;
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			finally
			{
			}
		}

		public class CountryViewModel1 : object
		{
			public CountryViewModel1() : base()
			{
			}

			public string NewName { get; set; }
		}

		public class CountryViewModel2 : object
		{
			public CountryViewModel2() : base()
			{
			}

			public string Name { get; set; }
		}

		private static void SomeFunction_3(object sender, System.EventArgs e)
		{
			Persistence.DatabaseContext databaseContext = null;

			try
			{
				databaseContext =
					new Persistence.DatabaseContext();

				//var varSomeData =
				//	from Country in DatabaseContext.Countries
				//	where (Country.Name.Contains("Iran"))
				//	orderby (Country.Name)
				//	select (new PartialCountry() { Name = Country.Name })
				//	;

				//var varSomeData =
				//	from Country in DatabaseContext.Countries
				//	where (Country.Name.Contains(txtCountryName.Text))
				//	orderby (Country.Name)
				//	select (new PartialCountry() { Name = Country.Name })
				//	;

				string countryName = string.Empty;
				//string countryName = countryNameTextBox.Text;

				var someData =
					from Country in databaseContext.Countries
					where Country.Name.Contains(countryName)
					orderby Country.Name
					select (new CountryViewModel1() { NewName = Country.Name })
					;

				string query = someData.ToString();

				foreach (CountryViewModel1 partialCountry in someData)
				{
					string name =
						partialCountry.NewName;
				}

				var data =
					databaseContext.Countries
					.Where(current => 1 == 1)
					;

				//var data =
				//	DatabaseContext.Countries
				//	.AsQueryable()
				//	;

				//var data =
				//	DatabaseContext.Countries
				//	.Select(current => new { current.Name })
				//	;

				//var data =
				//	DatabaseContext.Countries
				//	.Select(current => new PartialCountry() { Name = current.Name })
				//	;

				// بررسی شود
				//var data =
				//	DatabaseContext.Countries
				//	.Select(current => current.Name)
				//	;

				data =
					data
					.Where(current => current.Name.Contains("Iran"))
					;

				data =
					data
					.OrderBy(current => current.Name)
					;

				query =
					data.ToString();

				var varResult =
					data.ToList();
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			finally
			{
				if (databaseContext != null)
				{
					databaseContext.Dispose();
					databaseContext = null;
				}
			}
		}

		private static void SomeFunction_4(object sender, System.EventArgs e)
		{
			Persistence.DatabaseContext DatabaseContext = null;

			try
			{
				DatabaseContext =
					new Persistence.DatabaseContext();

				// Solution (1)
				//var data =
				//	DatabaseContext.Countries
				//	.AsQueryable()
				//	;

				//data =
				//	data
				//	.Where(current => current.Name == "Some Name")
				//	;

				//data =
				//	data
				//	.Where(current => current.Population >= 100)
				//	;

				//data =
				//	data
				//	.OrderBy(current => current.Name)
				//	;

				//string strQuery = data.ToString();

				//SELECT 
				//[Extent1].[Id] AS [Id], 
				//[Extent1].[Name] AS [Name], 
				//[Extent1].[Population] AS [Population]
				//FROM [dbo].[Countries] AS [Extent1]
				//WHERE (N'Some Name' = [Extent1].[Name]) AND ([Extent1].[Population] >= 100)
				//ORDER BY [Extent1].[Name] ASC

				// /Solution (1)

				// Solution (2)
				//var data =
				//	DatabaseContext.Countries
				//	.AsQueryable()
				//	;

				//data =
				//	data
				//	.Where(current => current.Name == "Some Name")
				//	;

				//data =
				//	data
				//	.OrderBy(current => current.Name)
				//	;

				//data =
				//	data
				//	.Where(current => current.Population >= 100)
				//	;

				//string strQuery = data.ToString();

				//SELECT 
				//[Extent1].[Id] AS [Id], 
				//[Extent1].[Name] AS [Name], 
				//[Extent1].[Population] AS [Population]
				//FROM [dbo].[Countries] AS [Extent1]
				//WHERE (N'Some Name' = [Extent1].[Name]) AND ([Extent1].[Population] >= 100)
				//ORDER BY [Extent1].[Name] ASC

				// /Solution (2)

				// Solution (3)
				//var data =
				//	DatabaseContext.Countries
				//	.AsQueryable()
				//	;

				//data = data
				//	.Where(current => current.Name == "Some Name")
				//	;

				//data = data
				//	.OrderBy(current => current.Name)
				//	.AsQueryable()
				//	;

				//data = data
				//	.Where(current => current.Population >= 100)
				//	;

				//string strQuery = data.ToString();

				//SELECT 
				//[Extent1].[Id] AS [Id], 
				//[Extent1].[Name] AS [Name], 
				//[Extent1].[Population] AS [Population]
				//FROM [dbo].[Countries] AS [Extent1]
				//WHERE (N'Some Name' = [Extent1].[Name]) AND ([Extent1].[Population] >= 100)
				//ORDER BY [Extent1].[Name] ASC

				// /Solution (3)

				// Solution (4)
				var data =
					DatabaseContext.Countries
					.AsQueryable()
					;

				data =
					data
					.Where(current => current.Name.StartsWith("A"))
					;

				data =
					data
					.OrderBy(current => current.Name)
					.AsQueryable()
					;

				data =
					data
					.Where(current => current.Name.EndsWith("Z"))
					;

				data =
					data
					.OrderBy(current => current.Population)
					.AsQueryable()
					;

				data =
					data
					.Where(current => current.Population >= 100)
					;

				string strQuery = data.ToString();

				// /Solution (4)
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			finally
			{
				if (DatabaseContext != null)
				{
					DatabaseContext.Dispose();
					DatabaseContext = null;
				}
			}
		}
	}
}
