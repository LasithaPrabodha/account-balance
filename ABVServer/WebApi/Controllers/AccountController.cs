using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        // GET: api/<AccountController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Account> accounts = _repository.Account.FindAll();
                _logger.LogInfo("loaded accounts");

                IEnumerable<AccountDto> accountDtos = _mapper.Map<AccountDto[]>(accounts);

                return Ok(accountDtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { error = e.Message });
            }
        }



        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAccount(int id)
        {

            try
            {
                Account account = _repository.Account.GetAccountWithDetails(id);

                AccountWithDetailsDto accountDto = _mapper.Map<AccountWithDetailsDto>(account);

                _logger.LogInfo("loaded account: " + id);
                return Ok(accountDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { error = e.Message });
            }
        }

        // POST api/<AccountController>/update
        [HttpPost("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateAccounts(int year, int month, IFormFile file)
        {
            try
            {
                if (file.FileName.EndsWith(".csv"))
                {
                    using (var sreader = new StreamReader(file.OpenReadStream()))
                    {
                        while (!sreader.EndOfStream)
                        {
                            string[] col = sreader.ReadLine().Split(',');
                            string accountName = col[0].ToString();
                            double amount = double.Parse(col[1].ToString());

                            Account account = _repository.Account.FindByCondition(account => account.AccountName.Contains(accountName)).FirstOrDefault();

                            Transaction transaction = new Transaction { AccountId = account.Id, Amount = amount, AddedDateTime = DateTime.Now, Month = month, Year = year };

                            _repository.Transaction.Create(transaction);
                            _logger.LogInfo("Transaction saved: " + transaction.Id);
                        }
                        _repository.Save();
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest(new { errMsg = "Wrong file format" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { error = e.Message });
            }


        }
    }
}