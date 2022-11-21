namespace Rankster.Data.Models;
[Table("RankItem")]
public partial class DbRankItem : BaseModel
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(Constants.StandardTextLength)]
    public string Name { get; set; }
    public string Description { get; set; }

    [ForeignKey(nameof(Rankster))]
    public Guid RanksterId { get; set; }

    public virtual DbRankster Rankster { get; set; }
    public virtual ICollection<DbVote> Votes { get; set; }

}
