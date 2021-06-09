using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Model;
using System;
using System.Linq;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseDomainService _purchaseDomainService;

        public PurchaseController(IPurchaseDomainService purchaseDomainService)
        {
            _purchaseDomainService = purchaseDomainService;
        }

        /// <summary>
        /// Process a user purchases a mask from a pharmacy;
        /// and handle all relevant data changes in an atomic transaction
        /// </summary>
        /// <param name="createModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPost]
        public ResponseModel<string> CreatePurchase(PurchaseCreateModel createModel)
        {
            try
            {
                var createResult = _purchaseDomainService.CreatePurchase(new PurchaseCreateEntity
                {
                    PharmacyId = createModel.PharmacyId,
                    UserId = createModel.UserId,
                    CreateDetailList = createModel.DetailList.Select(s => new PurchaseCreateDetailEntity
                    {
                        PharmacyProductId = s.PharmacyProductId,
                        Quantity = s.Quantity,
                        Amount = s.Amount
                    }).ToList()
                });

                if (createResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "create purchase success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "create purchase fail."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
