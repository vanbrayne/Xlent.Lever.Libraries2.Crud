namespace Xlent.Lever.Libraries2.Crud.Mappers
{
    /// <summary>
    /// Indicates that the implementor has one or more crud methods./>.
    /// </summary>
    public interface IMappable
    {
    }

    /// <inheritdoc />
    public interface IMappable<in TModel, in TId> : IMappable
    {
    }
}
