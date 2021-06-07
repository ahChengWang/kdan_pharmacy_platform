using Mapster;
using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BalanceController : ControllerBase
    {
        private readonly BalanceDomainService _balanceDomainService;

        public BalanceController(BalanceDomainService balanceDomainService)
        {
            _balanceDomainService = balanceDomainService;
        }

        /// <summary>
        /// The top x users by total transaction amount of masks within a date range
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        [HttpGet]
        public List<UserTranHistoryModel> GetUserTransactionHistory([FromQuery] DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            try
            {
                return _balanceDomainService.GetUserTranSummaryByOption(dateTimeFrom, dateTimeTo).Select(s => s.Adapt<UserTranHistoryModel>()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The total amount of masks and dollar value of transactions that happened within a date range
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        [HttpGet]
        public TransactionSummaryModel GetMasksTransactionSummary([FromQuery] DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            try
            {
                var result = _balanceDomainService.GetMasksTranSummary(dateTimeFrom, dateTimeTo).Select(s => s.Adapt<MaskTransactionSummaryModel>()).ToList();

                return new TransactionSummaryModel
                {
                    MaskTranSummaryList = result,
                    TotalTransactionAmount = result.Sum(sum => sum.TotalMaskTranAmount)
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
