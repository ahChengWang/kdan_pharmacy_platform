using System.Collections.Generic;

namespace PharmacyMask.Model
{
    public class TransactionSummaryModel
    {
        public List<MaskTransactionSummaryModel> MaskTranSummaryList { get; set; }
        public decimal TotalTransactionAmount { get; set; }
    }
}
