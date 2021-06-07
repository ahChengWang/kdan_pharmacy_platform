using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Model;
using System;

namespace PharmacyMask.Controller
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
        [HttpPut]
        public string UpdateMask(ProductMaskEditModel maskEditModel)
        {
            try
            {
                var updResult = _productDomainService.UpdateMask(new ProductEntity
                {
                    Id = maskEditModel.MaskId,
                    ProductName = maskEditModel.MaskName
                });

                if (updResult)
                    return "update mask success.";
                else
                    return "update mask fail.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
