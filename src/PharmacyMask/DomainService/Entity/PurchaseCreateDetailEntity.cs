namespace PharmacyMask.DomainService.Entity
{
    public class PurchaseCreateDetailEntity
    {
        public int PharmacyProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
