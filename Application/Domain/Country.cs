namespace Domain
{
	public class Country : Base.Entity
	{
		#region Configuration
		/// <summary>
		/// Nested Class
		/// </summary>
		internal class Configuration : object,
			Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Country>
		{
			public Configuration() : base()
			{
			}

			public void Configure
				(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Country> builder)
			{
				//builder
				//	.HasMany(country => country.States)
				//	.WithOne(state => state.Country)
				//	.IsRequired(required: true)
				//	.HasForeignKey(state => state.CountryId)
				//	.OnDelete(deleteBehavior:
				//		Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction)
				//	;

				builder
					.HasMany(current => current.States)
					.WithOne(other => other.Country)
					.IsRequired(required: true)
					.HasForeignKey(other => other.CountryId)
					.OnDelete(deleteBehavior:
						Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction)
					;
			}
		}
		#endregion /Configuration

		public Country() : base()
		{
		}

		// **********
		public int Code { get; set; }
		// **********

		// **********
		/// <summary>
		/// به میلیون نفر
		/// </summary>
		public int Population { get; set; }
		// **********

		// **********
		public int HealthyRate { get; set; }
		// **********

		// **********
		[System.ComponentModel.DataAnnotations.StringLength
			(maximumLength: 50)]
		public string Name { get; set; }
		// **********

		// **********
		public virtual System.Collections.Generic.IList<State> States { get; set; }
		// **********
	}
}
