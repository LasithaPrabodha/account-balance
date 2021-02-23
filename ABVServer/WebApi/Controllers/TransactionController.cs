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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionController(ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<TransactionController>
        [HttpGet]
        public IActionResult Get()
        {

            try
            {
                IQueryable<Transaction> transactions = _unitOfWork.Transaction.FindAll();
                _logger.LogInfo("loaded transactions");

                List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

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

            if(_startDate > _endDate)
            {
                return BadRequest(new Error { Message = "Start date is greater than end date." });
            }

            try
            {
                IEnumerable<Transaction> transactions = _unitOfWork.Transaction.FindByCondition(transaction=>
                    transaction.TransactionDate.Date <= _endDate && transaction.TransactionDate.Date >= _startDate
                );

                List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
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
