using Ambev.DeveloperEvaluation.Application.Sales.AddSale;
using Ambev.DeveloperEvaluation.Application.Sales.AlterSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing sales operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public SalesController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] AddSaleCommand command)
        {
            _logger.Information("Received request to create a new sale with details: {@Command}", command);

            var result = await _mediator.Send(command);
            if (result == Guid.Empty)
            {
                _logger.Error("Failed to create sale with details: {@Command}", command);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Failed to create sale"
                });
            }

            return Created(string.Empty, new ApiResponseWithData<Guid>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = result
            });
        }

        /// <summary>
        /// Updates an existing sale
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] AlterSaleCommand command)
        {
            _logger.Information("Received request to update sale with details: {@Command}", command);

            var result = await _mediator.Send(command);

            if (result is false)
            {
                _logger.Error($"Sale with number {command.SaleNumber} not found.");
                return NotFound(new ApiResponse
                {
                    Success = result,
                    Message = $"Sale with number {command.SaleNumber} not found."
                });
            }

            return Ok(new ApiResponse { Success = result, Message = "Sale updated successfully" });
        }

        /// <summary>
        /// Retrieves sales based on the provided filters
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetSalesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetSalesQuery query)
        {
            _logger.Information("Received request to get sales with filters: {@Query}", query);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a sale based on the provided sale number
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteSaleCommand command)
        {
            _logger.Information("Received request to delete sale with number: {SaleNumber}", command.SaleNumber);

            var result = await _mediator.Send(command);
            if (result is false)
            {
                return NotFound(new ApiResponse { Success = result, Message = $"Sale with number {command.SaleNumber} not found." });
            }

            return Ok(new ApiResponse { Success = result, Message = "Sale canceled successfully" });
        }

    }
}