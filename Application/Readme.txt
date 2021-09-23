----------------------------------------------------------------------------------------------------
Lazy Loading:

(1):

	Install-Package Microsoft.EntityFrameworkCore.Proxies

		Microsoft.EntityFrameworkCore.Proxies

			Castle.Core
			Microsoft.EntityFrameworkCore

(2):

	protected override void OnConfiguring
		(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
	{
		//base.OnConfiguring(optionsBuilder);

		string connectionString = "...";

		optionsBuilder
			.UseLazyLoadingProxies()
			.UseSqlServer(connectionString: connectionString)
			;
		;
	}

(3):

	Note: All navigation properties should have [virtual] keyword!
	Note: All class contains navigation properties should have [public] or [protected] constructor!
----------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------
Learning Anonymous Object:
----------------------------------------------------------------------------------------------------

(1)
class Person
{
	public int Age { get; set; }
	public string FullName { get; set; }
}

در جای دیگری

Person thePerson = new Person();

thePerson.Age = 20;
thePerson.FullName = "Ali Reza Alavi";

----------------------------------------------------------------------------------------------------

(2)
var thePerson = new { Age = 20, FullName = "Ali Reza Alavi" };

----------------------------------------------------------------------------------------------------
