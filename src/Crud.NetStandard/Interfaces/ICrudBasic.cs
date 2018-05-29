namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ICrud{TModelCreate, TModel,TId}" />
    public interface ICrudBasic<TModel, TId> :
        ICrudBasic<TModel, TModel, TId>,
        ICreate<TModel, TId>
    {
    }

    /// <summary>
    /// Interface for CRUD operations."/>.
    /// </summary>
    public interface ICrudBasic<in TModelCreate, TModel, TId> :
        ICreate<TModelCreate, TModel, TId>,
        IRead<TModel, TId>,
        IReadAll<TModel, TId>,
        IUpdate<TModel, TId>,
        IDelete<TId>,
        IDeleteAll
        where TModel : TModelCreate
    {
    }
}
