using AutoMapper;
using Contracts;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
                return StatusCode(500, new Error { Message = e.Message });
            }
        }

        [HttpGet("range")]
        public IActionResult GetTransactionForRange(string startDate, string endDate)
        {
            DateTime _endDate = DateTime.Parse(endDate).Date;
            DateTime _startDate  = DateTime.Parse(startDate).Date;

            try
            {
                IEnumerable<Transaction> transactions = _repository.Transaction.FindByCondition(transaction=>
                    transaction.TransactionDate.Date <= _endDate && transaction.TransactionDate.Date >= _startDate
                );

                IEnumerable<TransactionDto> transactionDtos = _mapper.Map<TransactionDto[]>(transactions);
                _logger.LogInfo("loaded transaction");
                return Ok(transactionDtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new Error { Message = e.Message });
            }
        }

    }
}
