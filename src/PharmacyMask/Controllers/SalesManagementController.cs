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
    public class SalesManagementController : ControllerBase
    {
        private readonly IPharmacyDomainService _pharmacyDomainService;
        private readonly ISalesManagementDomainService _salesManagementDomainService;

        public SalesManagementController(IPharmacyDomainService pharmacyDomainService,
            ISalesManagementDomainService salesManagementDomainService)
        {
            _pharmacyDomainService = pharmacyDomainService;
            _salesManagementDomainService = salesManagementDomainService;
        }

        /// <summary>
        /// List all masks that are sold by a given pharmacy, sorted by mask name or mask price
        /// </summary>
        /// <param name="searchModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<List<PharmacyMasksModel>> GetMasksByPharmacy([FromQuery] PharmacyMaskSearchModel searchModel)
        {
            try
            {
                var pharmacyInfoList = _pharmacyDomainService.GetPharmacyInfo(null, searchModel.PharmacyName);

                var pharmacyProductMaskList = _salesManagementDomainService.GetPharmacyProductMaskList(new ProductSearchEntity
                {
                    ProductName = searchModel.MaskName,
                    PriceFrom = searchModel.PriceFrom,
                    PriceTo = searchModel.PriceTo
                },
                pharmacyInfoList);

                var result = pharmacyProductMaskList.Select(s => new PharmacyMasksModel
                {
                    PharmacyName = s.PharmacyName,
                    MaskName = s.MaskName,
                    Color = s.Color,
                    PerPack = s.QtyPerPack,
                    Price = s.Price
                }).ToList();

                switch (searchModel.OrderTermId)
                {
                    case OrderTermEnum.MaskName:
                        result.OrderBy(ob => ob.MaskName);
                        break;
                    case OrderTermEnum.MaskPrice:
                        result.OrderBy(ob => ob.Price);
                        break;
                    default:
                        break;
                }

                return new ResponseModel<List<PharmacyMasksModel>>
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List all pharmacies that have more or less than x mask products within a price range
        /// </summary>
        /// <param name="searchModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<List<PharmaciesMaskSummaryModel>> GetPharmaciesMaskSummary([FromQuery] PharmacyMaskSearchModel searchModel)
        {
            try
            {
                var pharmacyInfoList = _pharmacyDomainService.GetPharmacyInfo(null, null);

                var pharmacyProductMaskList = _salesManagementDomainService.GetPharmacyProductMaskSummary(new ProductSearchEntity
                {
                    ProductName = "",
                    PriceFrom = searchModel.PriceFrom,
                    PriceTo = searchModel.PriceTo
                },
                pharmacyInfoList);

                var result = pharmacyProductMaskList.Select(s => new PharmaciesMaskSummaryModel
                {
                    PharmacyName = s.PharmacyName,
                    MaskCnt = s.MaskProductCnt
                }).ToList();

                return new ResponseModel<List<PharmaciesMaskSummaryModel>>
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Search for pharmacies or masks by name, ranked by relevance to search term
        /// </summary>
        /// <param name="searchModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<List<PharmacyMasksModel>> GetPharmaciesMaskBySearchTerm([FromQuery] PharmacyMaskSearchModel searchModel)
        {
            try
            {
                var pharmacyInfoList = _pharmacyDomainService.GetPharmacyInfo(null, searchModel.PharmacyName);

                var pharmacyProductMaskList = _salesManagementDomainService.GetPharmacyProductMaskList(new ProductSearchEntity
                {
                    ProductName = searchModel.MaskName,
                    PriceFrom = searchModel.PriceFrom,
                    PriceTo = searchModel.PriceTo
                },
                pharmacyInfoList);

                var result = pharmacyProductMaskList.Select(s => new PharmacyMasksModel
                {
                    PharmacyName = s.PharmacyName,
                    MaskName = s.MaskName,
                    Color = s.Color,
                    PerPack = s.QtyPerPack,
                    Price = s.Price
                }).ToList();


                switch (searchModel.SearchTermId)
                {
                    case OrderTermEnum.PharmacyName:
                        result.OrderBy(ob => ob.PharmacyName);
                        break;
                    case OrderTermEnum.MaskName:
                        result.OrderBy(ob => ob.MaskName);
                        break;
                    default:
                        break;
                }

                return new ResponseModel<List<PharmacyMasksModel>>
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit mask price
        /// </summary>
        /// <param name="productEditModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPut]
        public ResponseModel<string> UpdatePrice(PharmacyProductEditModel productEditModel)
        {
            try
            {
                var updResult = _salesManagementDomainService.UpdatePrice(new PharmacyProductEntity
                {
                    Id = productEditModel.PharmacyProductId,
                    Price = productEditModel.Price
                });

                if (updResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "update price success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "update price fail."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Remove a mask product from a pharmacy given by mask name
        /// </summary>
        /// <param name="productDeleteModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpDelete]
        public ResponseModel<string> DeleteProduct(ProductDeleteModel productDeleteModel)
        {
            try
            {
                var delResult = _salesManagementDomainService.DeleteProduct(new PharmacyProductEntity
                {
                    PharmacyId = productDeleteModel.PharmacyId,
                    ProductName = productDeleteModel.DeleteProductName
                });

                if (delResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "delete pharmacy mask success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "delete pharmacy mask fail."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
