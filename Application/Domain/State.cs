namespace Domain
{
	public class State : Base.Entity
	{
		#region Configuration
		internal class Configuration : object,
			Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<State>
		{
			public Configuration() : base()
			{
			}

			public void Configure
				(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<State> builder)
			{
				builder
					.HasMany(current => current.Cities)
					.WithOne(other => other.State)
					.IsRequired(required: true)
					.HasForeignKey(other => other.StateId)
					.OnDelete(deleteBehavior:
						Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction)
					;
			}
		}
		#endregion /Configuration

		public State() : base()
		{
		}

		// **********
		// **********
		// **********
		public System.Guid CountryId { get; set; }
		// **********

		// **********
		public virtual Country Country { get; set; }
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

		// **********
		public virtual System.Collections.Generic.IList<City> Cities { get; set; }
		// **********
	}
}
