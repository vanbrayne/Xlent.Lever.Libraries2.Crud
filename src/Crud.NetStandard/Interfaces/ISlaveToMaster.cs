namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Operations for reading and deleting the children of a parent.
    /// </summary>
    public interface ISlaveToMaster<TModel, in TId> :
        IReadChildren<TModel, TId>,
        IDeleteChildren<TId>
    {
    }
}
