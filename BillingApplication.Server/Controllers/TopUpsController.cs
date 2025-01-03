﻿using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Server.Services.Manager.TopUpsManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BillingApplication.Server.Controllers
{
    [Route("topups")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    public class TopUpsController : ControllerBase
    {
        public readonly ITopUpsManager topUpsManager;
        public readonly ILogger<TopUpsController> logger;

        public TopUpsController(ITopUpsManager topUpsManager, ILogger<TopUpsController> logger)
        {
            this.topUpsManager = topUpsManager;
            this.logger = logger;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TopUps model)
        {
            try
            {
                int? topupId = await topUpsManager.AddTopUp(model);
                logger.LogInformation($"ADDING: TopUp {topupId} added");
                return Created();
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR ADDING: TopUp has not been added" +
                                $"\nMessage:{ex.Message}" +
                                $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await topUpsManager.GetTopUps();
                logger.LogInformation($"GETTING: TopUps recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: TopUps has not been recieved" +
                                $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await topUpsManager.GetTopUpById(id);
                logger.LogInformation($"GETTING: TopUp {id} recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: TopUp {id} has not been recieved" +
                                $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var result = await topUpsManager.GetTopUpsByUserId(userId);
                logger.LogInformation($"GETTING: User's {userId} TopUps recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: User's {userId} TopUps has not been recieved" +
                                $"\nMessage:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
