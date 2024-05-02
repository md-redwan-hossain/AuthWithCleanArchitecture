namespace AuthWithCleanArchitecture.Domain.Common;

public abstract class Timestamp
{
    private bool _isCreatedAtUtcAssigned;
    private DateTime _createdAtUtc;

    public required DateTime CreatedAtUtc
    {
        get => _createdAtUtc;
        set
        {
            if (_isCreatedAtUtcAssigned || CreatedAtUtc != default) return;
            _createdAtUtc = value;
            _isCreatedAtUtcAssigned = true;
        }
    }

    public DateTime? UpdatedAtUtc { get; set; }
}