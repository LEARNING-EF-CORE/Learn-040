namespace Domain
{
	public class City : Base.Entity
	{
		#region Configuration
		internal class Configuration : object,
			Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<City>
		{
			public Configuration() : base()
			{
			}

			public void Configure
				(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<City> builder)
			{
			}
		}
		#endregion /Configuration

		public City() : base()
		{
		}

		// **********
		// **********
		// **********
		public System.Guid StateId { get; set; }
		// **********

		// **********
		public virtual State State { get; set; }
		// **********
		// **********
		// **********

		// **********
		public int Code { get; set; }
		// **********

		// **********
		[System.ComponentModel.DataAnnotations.StringLength
			(maximumLength: 50)]
		public string Name { get; set; }
		// **********
	}
}
