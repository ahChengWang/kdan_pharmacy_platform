using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Model;
using System;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDomainService _productDomainService;

        public ProductController(ProductDomainService productDomainService)
        {
            _productDomainService = productDomainService;
        }

        /// <summary>
        /// Edit mask name
        /// </summary>
        /// <param name="maskEditModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPut]
        public ResponseModel<string> UpdateMask(ProductMaskEditModel maskEditModel)
        {
            try
            {
                var updResult = _productDomainService.UpdateMask(new ProductEntity
                {
                    Id = maskEditModel.MaskId,
                    ProductName = maskEditModel.MaskName
                });


                if (updResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "update mask success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "update mask fail."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
