namespace Xlent.Lever.Libraries2.Crud.Interfaces
{
    /// <summary>
    /// Functionality for persisting objects that has no life of their own, but are only relevant with their master.
    /// Examples: A list of rows on an invoice, a list of attributes of an object, the contact details of a person.
    /// </summary>
    public interface ISlaveToMaster<TModel, in TId> : IManyToOne<TModel, TId>
    {
    }
}
