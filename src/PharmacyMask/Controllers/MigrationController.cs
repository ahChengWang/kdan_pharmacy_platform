using Microsoft.AspNetCore.Mvc;
using PharmacyMask.BackOffice.Model;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]

    public class MigrationController : ControllerBase
    {
        private readonly IPharmacyDomainService _pharmacyDomainService;
        private readonly IMaskService _maskDomainService;
        private readonly IUserDomainService _userDomainService;

        public MigrationController(PharmacyDomainService pharmacyDomainService,
            MaskService maskDomainService,
            UserDomainService userDomainService
            )
        {
            _pharmacyDomainService = pharmacyDomainService;
            _maskDomainService = maskDomainService;
            _userDomainService = userDomainService;
        }

        /// <summary>
        /// insert pharmacy and mask data
        /// </summary>
        /// <param name="pharmaciesModel">import pharmacies.json </param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPost]
        public ResponseModel<string> InsertPharmaciesMaskData(List<PharmaciesMigraModel> pharmaciesModel)
        {
            try
            {
                var pharmacyMigraResult = _pharmacyDomainService.MigrationPharmacy(
                    pharmaciesModel.Select(s =>
                    {
                        return new PharmacyMigrationEntity
                        {
                            PharmacyName = s.Name,
                            CashBalance = s.CashBalance,
                            OpeningHours = s.OpeningHours
                        };
                    }).ToList());

                var maskMigraResult = _maskDomainService.MigrationMask(
                    pharmaciesModel.SelectMany(sm =>
                        sm.Masks.Select(s =>
                        {
                            return new MaskMigrationEntity
                            {
                                PharmacyName = sm.Name,
                                MaskDesc = s.Name,
                                Price = s.Price
                            };
                        })).ToList());

                if (pharmacyMigraResult && maskMigraResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "insert data success"
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "insert data faile"
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// insert user mask data
        /// </summary>
        /// <param name="userModel">import users.json </param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPost]
        public ResponseModel<string> InsertUserData(List<UserMigraModel> userModel)
        {
            try
            {
                var inserUserDataResult = _userDomainService.MigrationUser(
                    userModel.Select(s =>
                    {
                        return new UserMigrationEntity
                        {
                            Name = s.Name,
                            CashBalance = s.CashBalance,
                            PurchaseHistories = s.PurchaseHistories.Select(se => new PurchaseHistoryEntity
                            {
                                PharmacyName = se.PharmacyName,
                                Remark = se.MaskName,
                                TransactionAmount = se.TransactionAmount,
                                TransactionDate = DateTime.Parse(se.TransactionDate)
                            }).ToList()
                        };
                    }).ToList());


                if (inserUserDataResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "insert user data success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "insert user data faile."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
