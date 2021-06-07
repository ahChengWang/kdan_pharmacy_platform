using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Model;
using System;
using System.Linq;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseDomainService _purchaseDomainService;

        public PurchaseController(PurchaseDomainService purchaseDomainService)
        {
            _purchaseDomainService = purchaseDomainService;
        }

        /// <summary>
        /// Process a user purchases a mask from a pharmacy;
        /// and handle all relevant data changes in an atomic transaction
        /// </summary>
        /// <param name="createModel"></param>
        [HttpPost]
        public string CreatePurchase(PurchaseCreateModel createModel)
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

                if(createResult)
                    return "create purchase success.";
                else
                    return "create purchase fail.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
