using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("api/transaction")]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {

        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public TransactionController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        private TransactionWithDetailsDto GetTransactionDto(Transaction transaction)
        {
            Account account = _repository.Account.GetAccountById(transaction.AccountId);

            TransactionWithDetailsDto transactionDto = _mapper.Map<TransactionWithDetailsDto>(transaction);
            transactionDto.Account = _mapper.Map<AccountDto>(account);

            return transactionDto;
        }

        // GET: api/<TransactionController>
        [HttpGet]
        public IActionResult Get()
        {

            try
            {
                IEnumerable<Transaction> transactions = _repository.Transaction.FindAll();
                _logger.LogInfo("loaded transactions");

                IEnumerable<TransactionDto> transactionDtos = _mapper.Map<TransactionDto[]>(transactions);

                return Ok(transactionDtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = e.Message });
            }
        }



        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public IActionResult GetTransaction(int id)
        {

            try
            {
                Transaction transaction = _repository.Transaction.FindByCondition(transaction => transaction.Id.Equals(id)).FirstOrDefault();

                TransactionWithDetailsDto transactionDto = this.GetTransactionDto(transaction);
                _logger.LogInfo("loaded transaction");
                return Ok(transactionDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("month")]
        public IActionResult GetTransactionForMonth(int year, int month)
        {

            try
            {
                IEnumerable<Transaction> transactions = _repository.Transaction.FindByCondition(transaction => transaction.Month == month && transaction.Year == year);

                IEnumerable<TransactionDto> transactionDtos = _mapper.Map<TransactionDto[]>(transactions);
                _logger.LogInfo("loaded transaction");
                return Ok(transactionDtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("range")]
        public IActionResult GetTransactionForRange(int startYear, int startMonth, int endYear, int endMonth)
        {

            try
            {
                IEnumerable<Transaction> transactions = _repository.Transaction.FindByCondition(transaction =>
                        transaction.Month >= startMonth && transaction.Year >= startYear
                        && transaction.Month <= endMonth && transaction.Year <= endYear);

                IEnumerable<TransactionDto> transactionDtos = _mapper.Map<TransactionDto[]>(transactions);
                _logger.LogInfo("loaded transaction");
                return Ok(transactionDtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = e.Message });
            }
        }

    }
}
