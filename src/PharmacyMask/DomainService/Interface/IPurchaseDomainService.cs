using PharmacyMask.DomainService.Entity;

namespace PharmacyMask.DomainService
{
    public interface IPurchaseDomainService
    {
        bool CreatePurchase(PurchaseCreateEntity createListEntity);
    }
}