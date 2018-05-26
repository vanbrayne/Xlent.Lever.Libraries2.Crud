namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="ICrudManyToOne{TModelCreate,TModel,TId}" />
    public interface ICrudManyToOne<TModel, TId> : 
        ICrudManyToOne<TModel, TModel, TId>,
        IManyToOneCrd<TModel, TId>,
        ICrud<TModel, TId>
    {
    }

    /// <inheritdoc cref="IManyToOneCrd{TModelCreate, TModel,TId}" />
    public interface ICrudManyToOne<in TModelCreate, TModel, TId> :
        IManyToOneCrd<TModelCreate, TModel, TId>,
        IManyToOneRud<TModel, TId>,
        ICrud<TModelCreate, TModel, TId>
        where TModel : TModelCreate
    {
    }
}
