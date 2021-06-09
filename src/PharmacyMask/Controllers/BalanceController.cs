using Mapster;
using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.Fundation.Definition.Enum;
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
        private readonly IBalanceDomainService _balanceDomainService;

        public BalanceController(IBalanceDomainService balanceDomainService)
        {
            _balanceDomainService = balanceDomainService;
        }

        /// <summary>
        /// The top x users by total transaction amount of masks within a date range
        /// </summary>
        /// <param name="dateTimeFrom">查詢起日</param>
        /// <param name="dateTimeTo">查詢迄日</param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<List<UserTranHistoryModel>> GetUserTransactionHistory([FromQuery] DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            try
            {
                return new ResponseModel<List<UserTranHistoryModel>>()
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = _balanceDomainService.GetUserTranSummaryByOption(dateTimeFrom, dateTimeTo).Select(s => s.Adapt<UserTranHistoryModel>()).ToList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// The total amount of masks and dollar value of transactions that happened within a date range
        /// </summary>
        /// <param name="dateTimeFrom">查詢起日</param>
        /// <param name="dateTimeTo">查詢迄日</param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<TransactionSummaryModel> GetMasksTransactionSummary([FromQuery] DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            try
            {
                var result = _balanceDomainService.GetMasksTranSummary(dateTimeFrom, dateTimeTo).Select(s => s.Adapt<MaskTransactionSummaryModel>()).ToList();

                return new ResponseModel<TransactionSummaryModel>
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = new TransactionSummaryModel
                    {
                        MaskTranSummaryList = result,
                        TotalTransactionAmount = result.Sum(sum => sum.TotalMaskTranAmount)
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
