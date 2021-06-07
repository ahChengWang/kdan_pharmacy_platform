namespace PharmacyMask.DomainService.Entity
{
    public class BalanceUpdateEntity
    {
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
