namespace HattrickApp.Api.Common;

public abstract class EntityBase<T>
{
    public required T Id { get; set; }
    
    // Add row version, auditing, soft delete etc
}
