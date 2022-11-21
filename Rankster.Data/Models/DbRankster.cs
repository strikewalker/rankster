namespace Rankster.Data.Models;
[Index(nameof(Code), IsUnique = true)]
[Table("Rankster")]
public partial class DbRankster : BaseModel
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(Constants.StandardTextLength)]
    public string Name { get; set; }
    [MaxLength(10)]
    public string Code { get; set; }
    public string Description { get; set; }
    [MaxLength(100)]
    public string StrikeUsername { get; set; }
    [Precision(18, 2)]
    public decimal CostUsd { get; set; }
    [MaxLength(10)]
    public string? Currency { get; set; }
    public bool Ended { get; set; }
    public DateTime? Ending { get; set; }

    public virtual ICollection<DbRankItem> RankItems { get; set; }

}
