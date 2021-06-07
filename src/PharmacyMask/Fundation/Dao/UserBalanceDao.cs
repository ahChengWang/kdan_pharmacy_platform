using System;

namespace PharmacyMask.Fundation.Dao
{
    public class UserBalanceDao
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal CashBalance { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
