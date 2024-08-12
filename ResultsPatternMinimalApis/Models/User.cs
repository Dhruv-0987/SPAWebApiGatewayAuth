using System.ComponentModel.DataAnnotations;

namespace ResultsPatternMinimalApis.Models;

public class User
{
    [Key]
    public Guid Id { get; init; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}