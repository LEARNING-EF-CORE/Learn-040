namespace Domain.Base
{
	public interface IEntity<T>
	{
		T Id { get; set; }
	}

	//public interface IEntity
	//{
	//	System.Guid Id { get; set; }
	//}
}
