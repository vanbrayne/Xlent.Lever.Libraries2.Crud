namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IManyToOneCrd{TModelCreate, TModel,TId}" />
    public interface IManyToOneCrd<TModel, TId> : 
        IManyToOneCrd<TModel, TModel, TId>,
        ICrd<TModel, TId>
    {
    }

    /// <inheritdoc cref="IManyToOneRead{TModel,TId}" />
    public interface IManyToOneCrd<in TModelCreate, TModel, TId> :
        IManyToOneRead<TModel, TId>,
        ICrd<TModelCreate, TModel, TId>,
        IDeleteChildren<TId>
        where TModel : TModelCreate
    {
    }
}
