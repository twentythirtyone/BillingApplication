﻿using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Services.BundleManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Utilites;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class BundleController : ControllerBase
    {
        private readonly IBundleManager bundleManager;

        public BundleController(IBundleManager bundleManager)
        {
            this.bundleManager = bundleManager;
        }

        //[RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Bundle BundleModel)
        {
            try
            {
                var result = await bundleManager.CreateBundle(BundleModel);
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Bundle BundleModel)
        {
            try
            {
                var result = await bundleManager.UpdateBundle(BundleModel);
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpDelete("deletebyid/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                var result = await bundleManager.DeleteBundle(id);
                return Ok($"Тариф {result} был удалён");
            }
            catch (BundleNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await bundleManager.GetAllBundles();
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await bundleManager.GetBundleById(id);
                return Ok(result);
            }
            catch (BundleNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
