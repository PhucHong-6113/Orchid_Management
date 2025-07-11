using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Admin;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231_SE172426_Exercise23.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        } 

        [HttpGet("accounts")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("accounts/search")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> SearchAccounts([FromQuery] string query)
        {
            var accounts = await _accountService.SearchAccountsAsync(query);
            return Ok(accounts);
        }

        [HttpGet("accounts/{id}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);

            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpPut("accounts/{id}/toggle-status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ToggleAccountStatus(Guid id)
        {
            var success = await _accountService.ToggleAccountStatusAsync(id);

            if (!success)
                return NotFound(new { message = "Account not found" });

            return NoContent();
        }
    }
}