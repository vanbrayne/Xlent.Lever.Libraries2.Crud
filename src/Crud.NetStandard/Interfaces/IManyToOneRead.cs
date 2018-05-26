namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <inheritdoc cref="IReadChildren{TManyModel,TId}" />
    public interface IManyToOneRead<TManyModel, in TId> :
        IReadChildren<TManyModel, TId>,
        IRead<TManyModel, TId>
    {
    }
}
