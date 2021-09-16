namespace Domain.Base
{
	public abstract class Entity : object, IEntity<System.Guid>
	{
		public Entity() : base()
		{
			Id = System.Guid.NewGuid();
		}

		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
			(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public System.Guid Id { get; set; }
	}
}
