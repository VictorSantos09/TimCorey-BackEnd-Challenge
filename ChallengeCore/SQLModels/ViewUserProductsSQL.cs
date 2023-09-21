namespace ChallengeCore.SQLModels;
public class ViewUserProductsSQL
{
    public string PR_NAME { get; set; }
    public decimal PR_PRICE { get; set; }
    public DateTime UP_SOLD_DATE { get; set; }
    public int UP_AMOUNT { get; set; }
    public decimal UP_TOTAL { get; set; }
}