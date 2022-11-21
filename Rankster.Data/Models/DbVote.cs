namespace Rankster.Data.Models;
[Index(nameof(SessionKey))]
[Table("Vote")]
public partial class DbVote : BaseModel
{
    [Key]
    public Guid Id { get; set; }

    public Guid StrikeInvoiceId { get; set; }
    public Guid SessionKey { get; set; }
    [ForeignKey(nameof(RankItem))]
    public Guid RankItemId { get; set; }

    public DateTime? Paid { get; set; }

    [ForeignKey(nameof(TypeRef))]
    public VoteType Type { get; set; }

    //references
    public virtual EnumTable<VoteType> TypeRef { get; set; }
    public virtual DbRankItem RankItem { get; set; }

}
