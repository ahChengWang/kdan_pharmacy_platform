using System.Collections.Generic;

namespace PharmacyMask.BackOffice.Model
{
    public class UserMigraModel
    {
        public string Name { get; set; }
        public decimal CashBalance { get; set; }
        public List<UserPurchaseMigraModel> PurchaseHistories { get; set; }
    }
}
