using System;

namespace PharmacyMask.Fundation.Dao
{
    public class UserBalanceLogDao
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal PreBalance { get; set; }
        public decimal LastBalance { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
