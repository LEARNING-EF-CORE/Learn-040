using System.Linq;

namespace Application
{
	public static class LinqPhilosophy : object
	{
		static LinqPhilosophy()
		{
		}

		public static void WorkinngOnDatabase()
		{
			// TSQL
			var query =
				"SELECT * FROM Countries WHERE Population >= 25 AND Population <= 35 ORDER BY Name ASC";

			// ارسال به بانک اطلاعاتی برای بدست آوردن اطلاعات اشخاص
		}

		public static void WorkingOnFiles()
		{
			var path = "C:\\WINDOWS";

			var directoryInfo =
				new System.IO.DirectoryInfo(path: path);

			foreach (System.IO.FileInfo fileInfo in directoryInfo.GetFiles())
			{
				System.Console.WriteLine(fileInfo.Name);
			}

			foreach (System.IO.FileInfo fileInfo in directoryInfo.GetFiles())
			{
				if ((fileInfo.Length >= 25 * 1024) && (fileInfo.Length <= 35 * 1024))
				{
					System.Console.WriteLine(fileInfo.Name);
				}
			}

			// صورت مساله‌ای که چهارشاخ گاردان را پایین می‌آورد

			// حال می‌خواهیم تمام فایل‌هایی را نشان دهد که سایز آنها بین
			// بیست و پنج کیلو بایت تا سی و پنج کیلو بایت بوده
			// و مرتب شده بر حسب نام فایل‌ها باشد
		}

		public static void WorkingOnXml()
		{
			// XmlDocument, XmlReader,...
		}

		public static void WorkinngOnDatabaseWithLinq()
		{
			var databaseContext =
				new Persistence.DatabaseContext();

			var countries =
				databaseContext.Countries
				.Where(current => current.Population >= 25 && current.Population <= 35)
				.OrderBy(current => current.Name)
				.ToList()
				;
		}

		public static void WorkingOnFilesWithLinq()
		{
			string path = "C:\\WINDOWS";

			var directoryInfo =
				new System.IO.DirectoryInfo(path: path);

			var files =
				directoryInfo.GetFiles()
				.Where(current => current.Length >= 25 * 1024 && current.Length <= 35 * 1024)
				.OrderBy(current => current.Name)
				.ToList()
				;
		}
	}
}
