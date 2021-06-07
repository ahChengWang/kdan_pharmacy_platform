namespace PharmacyMask.BackOffice.Model
{
    public class UserPurchaseMigraModel
    {
        public string PharmacyName { get; set; }
        public string MaskName { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionDate { get; set; }
    }
}
