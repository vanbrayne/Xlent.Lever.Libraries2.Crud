using Xlent.Lever.Libraries2.Crud.Assert;

namespace Xlent.Lever.Libraries2.Crud.Test.NuGet.Model
{
    /// <summary>
    /// A minimal storable item that implments <see cref="IValidatable"/> to be used in testing
    /// </summary>
    public class TestItemValidated<TId> : TestItemId<TId>, IValidatable
    {
        public void Validate(string errorLocation, string propertyPath = "")
        {
            FulcrumValidate.IsNotNullOrWhiteSpace(Value, nameof(Value), errorLocation);
        }
    }
}
