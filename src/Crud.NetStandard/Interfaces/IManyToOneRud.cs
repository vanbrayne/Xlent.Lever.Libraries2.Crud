namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOneRead{TModel,TId}" />
    public interface IManyToOneRud<TModel, in TId> :
        IManyToOneRead<TModel, TId>,
        IRud<TModel, TId>,
        IDeleteChildren<TId>
    {
    }
}
