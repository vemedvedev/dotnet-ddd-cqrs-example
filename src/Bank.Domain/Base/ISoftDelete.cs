namespace Bank.Domain.Base;

public interface ISoftDelete
{
    public DateTime? DeletedAt { get;  }
}
