using Mapster;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Fundation.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace PharmacyMask.DomainService
{
    public class PharmacyDomainService
    {
        private readonly PharmacyRepository _pharmacyRepository;
        private readonly PharmacyDetailRepository _pharmacyInfoRepository;
        private readonly PharmacyBalanceRepository _pharmacyBalanceRepository;

        public PharmacyDomainService(PharmacyRepository pharmacyRepository,
            PharmacyDetailRepository pharmacyInfoRepository,
            PharmacyBalanceRepository pharmacyBalanceRepository)
        {
            _pharmacyRepository = pharmacyRepository;
            _pharmacyInfoRepository = pharmacyInfoRepository;
            _pharmacyBalanceRepository = pharmacyBalanceRepository;
        }

        private List<DayOfWeek> _dayOfWeekList = new List<DayOfWeek>
        {
            DayOfWeek.Sunday,DayOfWeek.Monday, DayOfWeek.Tuesday,DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday,DayOfWeek.Saturday
        };

        public List<PharmacyOpenTimeEntity> GetOpenDayTime(List<DayOfWeek> dayOfWeek)
        {
            var pharmacyList = GetPharmacyInfo(null, null);
            var pharmacyInfoList = _pharmacyInfoRepository.SelectByDay(dayOfWeek);

            return (from m in pharmacyList
                    join d in pharmacyInfoList
                    on m.Id equals d.PharmacyId
                    select new PharmacyOpenTimeEntity
                    {
                        PharmacyId = m.Id,
                        PharmacyName = m.Name,
                        DayOfWeekId = d.DayOfWeek,
                        DayOfWeek = d.DayOfWeek.ToString(),
                        OpenTime = d.OpenTime.ToString(),
                        CloseTime = d.CloseTime.ToString()
                    }).OrderBy(ob => ob.PharmacyId).ThenBy(tb => tb.DayOfWeekId).ToList();
        }

        public List<PharmacyEntity> GetPharmacyInfo(List<int> pharmacyId, string pharmacyName)
            => _pharmacyRepository.SelectByOption(pharmacyId, pharmacyName).Select(s => s.Adapt<PharmacyEntity>()).ToList();

        public bool UpdatePharmacyName(int pharmacyId, string pharmacyName)
        {
            var updResult = true;

            using (var scope = new TransactionScope())
            {
                updResult = _pharmacyRepository.UpdateName(pharmacyId, pharmacyName) == 1;
                if (updResult)
                    scope.Complete();
            }

            return updResult;
        }


        public bool MigrationPharmacy(List<PharmacyMigrationEntity> pharmacyMigraEntity)
        {
            var migraResult = true;
            var pharmacyList = new List<PharmacyDao>();
            var pharmacyInfoList = new List<PharmacyDetailDao>();
            var pharmacyBalanceList = new List<PharmacyBalanceDao>();
            var userList = new List<UserDao>();
            var userTransactionHistoryList = new List<UserTransactionHistoryDao>();

            // pharmacy data
            pharmacyList = pharmacyMigraEntity.Select(s =>
            {
                return new PharmacyDao
                {
                    Name = s.PharmacyName,
                    Status = PharmacyStatusEnum.BusinessAsUsual,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            // pharmacy_info data
            var tmpPharmacyDetailList = pharmacyMigraEntity.SelectMany(s =>
            {
                // openingHours 切分
                var openTimeList = s.OpeningHours.Split('/').ToList();

                var openTimeDic = new List<Dictionary<DayOfWeek, (DateTime, DateTime)>>();

                return openTimeList.SelectMany(se =>
                {
                    var src = se.Split(' ').ToList();

                    var day = ConvertDayStr(
                        // 解析每個營業時間, 將星期與時間切分, 篩選為日期的
                        src.Where(w => w.Length > 2 && _dayOfWeekList.Select(x => x.ToString().ToUpper().Substring(0, 3)).Contains(w.Substring(0, 3).ToUpper())),
                        // 篩選星期是否為一個區間的
                        src.Count(c => c == "-") > 1
                        );

                    var time = src.Where(w => w.Length > 2 && int.TryParse(w.Substring(0, 2), out _)).Select(s => Convert.ToDateTime(s)).ToArray();

                    return day.Select(sel => new
                    {
                        Name = s.PharmacyName,
                        DayOfWeek = sel,
                        OpenTime = time[0],
                        CloseTime = time[1],
                        CreateUser = "system",
                        UpdateUser = "system"
                    }).ToList();
                });

            }).ToList();

            // pharmacy_balance data
            var tmpPharmacyBalanceList = pharmacyMigraEntity.Select(s =>
            {
                return new
                {
                    Name = s.PharmacyName,
                    CashBalance = s.CashBalance,
                    CreateUser = "system",
                    UpdateUser = "system"
                };
            }).ToList();

            using (var scope = new TransactionScope())
            {
                bool insResult = false;

                insResult = _pharmacyRepository.Insert(pharmacyList) == pharmacyList.Count();
                if (!insResult) migraResult = false;

                var pharmacyData = _pharmacyRepository.SelectAll();

                pharmacyInfoList = tmpPharmacyDetailList.Select(s =>
                {
                    return new PharmacyDetailDao
                    {
                        PharmacyId = pharmacyData.FirstOrDefault(f => f.Name == s.Name).Id,
                        DayOfWeek = s.DayOfWeek,
                        OpenTime = s.OpenTime.TimeOfDay,
                        CloseTime = s.CloseTime.TimeOfDay,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _pharmacyInfoRepository.Insert(pharmacyInfoList) == pharmacyInfoList.Count();
                if (!insResult) migraResult = false;

                pharmacyBalanceList = tmpPharmacyBalanceList.Select(s =>
                {
                    return new PharmacyBalanceDao
                    {
                        PharmacyId = pharmacyData.FirstOrDefault(f => f.Name == s.Name).Id,
                        CashBalance = s.CashBalance,
                        CreateUser = s.CreateUser,
                        UpdateUser = s.UpdateUser
                    };
                }).ToList();

                insResult = _pharmacyBalanceRepository.Insert(pharmacyBalanceList) == pharmacyBalanceList.Count();
                if (!insResult) migraResult = false;

                if (migraResult)
                    scope.Complete();
            }

            return migraResult;
        }



        #region private method

        /// <summary>
        /// 將輸入的星期 前三碼轉換為 星期Enum
        /// </summary>
        /// <param name="sourceDayStr"></param>
        /// <param name="isRange"></param>
        /// <returns></returns>
        private List<DayOfWeek> ConvertDayStr(IEnumerable<string> sourceDayStr, bool isRange)
        {
            var tmp = sourceDayStr.Select(s =>
            {
                var day = new DayOfWeek?();

                switch (s.Substring(0, 3).ToUpper())
                {
                    case "SUN":
                        day = DayOfWeek.Sunday;
                        break;
                    case "MON":
                        day = DayOfWeek.Monday;
                        break;
                    case "TUE":
                        day = DayOfWeek.Tuesday;
                        break;
                    case "WED":
                        day = DayOfWeek.Wednesday;
                        break;
                    case "THU":
                        day = DayOfWeek.Thursday;
                        break;
                    case "FRI":
                        day = DayOfWeek.Friday;
                        break;
                    case "SAT":
                        day = DayOfWeek.Saturday;
                        break;
                    default:
                        break;
                };

                return (DayOfWeek)day;
            }).ToList();

            if (isRange)
                tmp = AddRangeDay(tmp.ToArray()[0], tmp.ToArray()[1], tmp);

            return tmp;
        }

        /// <summary>
        /// 若open hours為一段區間, 補上中間的星期
        /// ex.open mon - thu, add tuesday、wednesday
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="srcDayList"></param>
        /// <returns></returns>
        private List<DayOfWeek> AddRangeDay(DayOfWeek startDay, DayOfWeek endDay, List<DayOfWeek> srcDayList)
        {
            for (int i = (int)startDay; (i + 1) < (int)endDay; i++)
            {
                srcDayList.Add((DayOfWeek)(i + 1));
            }

            return srcDayList;
        }
        #endregion
    }
}
