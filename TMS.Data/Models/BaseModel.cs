using System.ComponentModel.DataAnnotations;

namespace TMS.Data.Models;

public abstract class BaseModel<TKey>
{
    [Key] public TKey Id { get; set; }

    public bool Deleted { get; set; } = false;
}