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
