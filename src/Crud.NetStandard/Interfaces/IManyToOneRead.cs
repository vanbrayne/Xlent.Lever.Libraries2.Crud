namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IReadChildren{TModel,TId}" />
    public interface IManyToOneRead<TModel, in TId> :
        IReadChildren<TModel, TId>,
        IRead<TModel, TId>
    {
    }
}
