using System.Linq;
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
			GetTSql();

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
				//N Fields
				// ----------

				// ----------
				//1 Field -> Search

				//Full Name ___________

				//[Search]

				// 2 -> if / else
				// ----------

				// ----------
				//2 Fields -> Search

				//Full Name ___________
				//Address   ___________

				//[Search]

				// 4 -> if / else
				// ----------

				// ----------
				// M Fields for Searching
				//  M
				// 2   -> if / else
				// ----------

				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					string countryNameString = string.Empty;
					string countryCodeToString = string.Empty;
					string countryCodeFromString = string.Empty;
					// **************************************************

					// **************************************************
					//var data =
					//	DatabaseContext.Countries
					//		.Where(current => current.IsActive)
					//		;

					// در نسخه‌های قدیمی
					//var data =
					//	DatabaseContext.Countries
					//		.Where(current => 1 == 1)
					//		;

					// در نسخه‌های جدید
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

					//data
					//	.Load();

					// يا

					// Note: Wrong Usage!
					//data = data.ToList();

					var result =
						data.ToList()
						;

					// result معادل DatabaseContext.Countries.Local
				}
				// **************************************************

				// **************************************************
				{
					string search = "   Ali       Reza  Iran Carpet                 Ali         ";

					string[] keywords =
						{ "Ali", "Reza", "Iran", "Carpet" }; // یه جوری

					var dataTemp =
						DatabaseContext.Countries
						.AsQueryable()
						;

					foreach (var keyword in keywords)
					{
						dataTemp =
							dataTemp
							.Where(current => current.Name.ToLower().Contains(keyword.ToLower()))
							;
					}

					dataTemp =
						dataTemp
						.OrderBy(current => current.Code)
						;

					var result =
						dataTemp
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					string search = "   Ali       Reza  Iran Carpet                 Ali         ";

					if (string.IsNullOrWhiteSpace(search))
					{
						return;
					}

					search =
						search.Trim();

					//search = "Ali       Reza  Iran Carpet                 Ali";

					while (search.Contains("  "))
					{
						search =
							search.Replace("  ", " ");
					}

					//search = "Ali Reza Iran Carpet Ali";

					//string[] keywords = search.Split(' ');

					//keywords = { "Ali", "Reza", "Iran", "Carpet", "Ali" }

					var keywords =
						search.Split(' ').Distinct();

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

					var result =
						dataTemp
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					// روش اول
					// دستورات ذيل کاملا با هم معادل هستند
					DatabaseContext.Countries
						.Load();
				}

				{
					// روش دوم
					// LINQ: Lambda Expression, Arrow Function
					var result =
						DatabaseContext.Countries
						.ToList()
						;
				}

				{
					// روش سوم
					// LINQ: Traditional
					var result =
						from Country in DatabaseContext.Countries
						select Country
						;
				}

				// SELECT * FROM Countries
				// **************************************************

				// **************************************************
				{
					// ها Country آرایه‌ای از
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.ToLower().Contains("Iran".ToLower())
						select Country
						;
				}
				// **************************************************

				// **************************************************
				{
					// ها Country آرایه‌ای از
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.ToLower().Contains("Iran".ToLower())
						orderby Country.Name
						select Country
						;

					foreach (Domain.Country item in result)
					{
						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************

				// **************************************************
				{
					// ها String آرایه‌ای از
					// (A)
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.ToLower().Contains("Iran".ToLower())
						orderby Country.Name
						select Country.Name
						;

					// SQL: SELECT Name FROM Countries

					// Note: Wrong Usage!
					//foreach (Models.Country item in result)
					//{
					//	System.Console.WriteLine(item.Name);
					//}

					foreach (string item in result)
					{
						System.Console.WriteLine(item);
					}
				}
				// **************************************************

				// **************************************************
				// Note: See Learning Anonymous Object File!
				// **************************************************

				// **************************************************
				{
					// ها Object آرایه ای از
					// (B)
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.ToLower().Contains("Iran".ToLower())
						orderby Country.Name
						select new { Name = Country.Name }
						;

					// SQL: SELECT Name FROM Countries

					// Note: Wrong Usage!
					//foreach (Models.Country item in result)
					//{
					//	System.Windows.Forms.MessageBox.Show(item.Name);
					//}

					// Note: Wrong Usage!
					//foreach (string item in result)
					//{
					//	System.Windows.Forms.MessageBox.Show(item);
					//}

					foreach (var item in result)
					{
						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************

				// **************************************************
				{
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select new { Country.Name }
						;

					foreach (var item in result)
					{
						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************

				// **************************************************
				{
					// (C)
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select new { Baghali = Country.Name }
						;

					// Note: Wrong Usage!
					//foreach (Models.Country item in result)
					//{
					//	System.Windows.Forms.MessageBox.Show(item.Name);
					//}

					foreach (var item in result)
					{
						System.Console.WriteLine(item.Baghali);
					}
				}
				// **************************************************

				// **************************************************
				{
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select new { Size = Country.Population, Country.Name }
						;

					// SQL: SELECT Name, Population FROM Countries

					foreach (var item in result)
					{
						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					// (D1)
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select (new CountryViewModel1() { NewName = Country.Name })
						;

					foreach (CountryViewModel1 item in result)
					{
						item.NewName += "!";

						System.Console.WriteLine(item.NewName);
					}
				}
				// **************************************************

				// **************************************************
				{
					// (D1)
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select (new CountryViewModel2 { Name = Country.Name })
						;

					foreach (CountryViewModel2 item in result)
					{
						item.Name += "!";

						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************

				// **************************************************
				{
					// (D3)
					// Note: Wrong Usage!
					//var result =
					//	from Country in DatabaseContext.Countries
					//	where Country.Name.Contains("Iran")
					//	orderby Country.Name
					//	select (new CountryViewModel2 { Country.Name })
					//	;

					//foreach (CountryViewModel2 item in result)
					//{
					//	item.Name += "!";

					//	System.Console.WriteLine(item.Name);
					//}
				}
				// **************************************************

				// **************************************************
				{
					// (E)
					// Note: متاسفانه کار نمی کند
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.Contains("Iran")
						orderby Country.Name
						select new Domain.Country { Name = Country.Name }
						;

					foreach (Domain.Country item in result)
					{
						System.Console.WriteLine(item.Name);
					}
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.ToList()
						;

					// "SELECT * FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (A)
					var result =
						DatabaseContext.Countries
						.Select(current => current.Name)
						.ToList()
						;

					// result -> string[]

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (B)
					var result =
						DatabaseContext.Countries
						.Select(current => new { Name = current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (B)
					var result =
						DatabaseContext.Countries
						.Select(current => new { current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (C)
					var result =
						DatabaseContext.Countries
						.Select(current => new { Baghali = current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (D)
					var result =
						DatabaseContext.Countries
						.Select(current => new CountryViewModel1() { NewName = current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (D)
					var result =
						DatabaseContext.Countries
						.Select(current => new CountryViewModel2 { Name = current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// Note: Wrong Usage!
					// It is similar to (D)
					//var result =
					//	DatabaseContext.Countries
					//	.Select(current => new CountryViewModel2() { current.Name })
					//	.ToList()
					//	;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					// It is similar to (E)
					// Note: متاسفانه کار نمی کند
					var result =
						DatabaseContext.Countries
						.Select(current => new Domain.Country { Name = current.Name })
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.Select(current => new { current.Name })
						.ToList()
						.Select(current => new Domain.Country
						{
							Name = current.Name,
						})
						.ToList()
						;

					// "SELECT Name FROM Countries"
				}
				// **************************************************

				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.Select(current => new { current.Id, current.Name })
						.ToList()
						.Select(current => new Domain.Country
						{
							Id = current.Id,
							Name = current.Name,
						})
						.ToList()
						;

					// "SELECT Id, Name FROM Countries"
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.Select(current => new
						{
							Id = current.Id,
							Name = current.Name,
							StateCount = current.States.Count,
						})
						.ToList()
						;
				}
				// **************************************************

				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.Select(current => new
						{
							current.Id,
							current.Name,
							StateCount = current.States.Count,
						})
						.ToList()
						;
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				{
					// در دو دستور ذیل در صورتی که تحت شرایطی تعداد
					// استان‌ها برای یک کشور صفر باشد، خطا ایجاد می‌شود
					var result =
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
				}

				{
					var result =
						DatabaseContext.Countries
						.Select(current => new
						{
							current.Id,
							current.Name,

							StateCount =
								current.States.Count,

							CityCount =
								current.States.Select(state => state.Cities.Count).Sum(),
						})
						.ToList()
						;
				}

				{
					var result =
						DatabaseContext.Countries
						.Select(current => new
						{
							current.Id,
							current.Name,

							StateCount =
								current.States.Count,

							CityCount =
								current.States.Count == 0 ? 0 :
								current.States.Select(state => new { XCount = state.Cities.Count }).Sum(x => x.XCount)

							//CityCount = current.States.Count == 0 ? 0 :
							//	current.States.Select(state => state.Cities.Count).Sum()

							//CityCount = current.States == null || current.States.Count == 0 ? 0 :
							//	current.States.Select(state => new { XCount = state.Cities == null ? 0 : state.Cities.Count }).Sum(x => x.XCount)
						})
						.ToList()
						;
				}

				{
					// مهدی اکبری
					var someData2600 =
						DatabaseContext.Countries
						.Select(current => new
						{
							current.Id,
							current.Name,

							StateCount =
								current.States.Count,

							CityCount =
								current.States.Select(state => state.Cities.Count).DefaultIfEmpty(0).Sum(),
						})
						.ToList()
						;
				}
				// **************************************************
				// **************************************************
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				// Group By
				// **************************************************
				{
					var result =
						DatabaseContext.Countries
						.GroupBy(current => current.Population)
						.Select(current => new
						{
							Population = current.Key,

							Count = current.Count(),
						})
						.ToList()
						;
				}

				{
					var result =
						DatabaseContext.Countries
						.Where(current => current.Population >= 120)
						.GroupBy(current => current.Population)
						.Select(current => new
						{
							Population = current.Key,

							Count = current.Count(),
						})
						.ToList()
						;
				}

				{
					var result =
						DatabaseContext.Countries
						.GroupBy(current => current.Population)
						.Select(current => new
						{
							Population = current.Key,

							Count = current.Count(),
						})
						.Where(ao => ao.Population >= 120)
						.ToList()
						;
				}

				{
					var result =
						DatabaseContext.Countries
						.Where(current => current.Name.ToLower().Contains("Iran".ToLower()))
						.GroupBy(current => current.Population)
						.Select(current => new
						{
							Population = current.Key,

							Count = current.Count(),
						})
						.ToList()
						;
				}

				{
					// Note: Wrong Usage!
					//var result =
					//	DatabaseContext.Countries
					//	.GroupBy(current => current.Population)
					//	.Select(current => new
					//	{
					//		Population = current.Key,

					//		Count = current.Count(),
					//	})
					//	.Where(ao => ao.Name.ToLower().Contains("Iran".ToLower()))
					//	.ToList()
					//	;
				}

				{
					var result =
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
				}

				{
					var result =
						DatabaseContext.Countries
						.Where(current => current.Name.ToLower().Contains("Iran".ToLower()))
						.GroupBy(current => current.Population)
						.Select(current => new
						{
							Population = current.Key,

							Count = current.Count(),
						})
						.Where(ao => ao.Count >= 30)
						.ToList()
						;
				}

				{
					var result =
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
				}

				{
					var result =
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
				}

				{
					var result =
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

		private static void GetTSql()
		{
			Persistence.DatabaseContext databaseContext = null;

			try
			{
				databaseContext =
					new Persistence.DatabaseContext();

				{
					var result =
						from Country in DatabaseContext.Countries
						where Country.Name.ToLower().Contains("Iran".ToLower())
						orderby Country.Name
						select (new { CountryName = Country.Name })
						;

					string sql =
						result.ToQueryString();
				}
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
	}
}
