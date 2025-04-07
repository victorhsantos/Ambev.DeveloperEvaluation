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
    [Produces("application/json")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">Mediator instance</param>
        /// <param name="logger">Logger instance</param>
        public SalesController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="command">Details of the sale to be created</param>
        /// <returns>Response with the ID of the created sale</returns>
        /// <response code="201">Sale created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [Consumes("application/json")]
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
        /// <param name="command">Details of the sale to be updated</param>
        /// <returns>Response indicating the result of the update operation</returns>
        /// <response code="200">Sale updated successfully</response>
        /// <response code="404">Sale not found</response>
        [HttpPut]
        [Consumes("application/json")]
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
        /// <param name="query">Filters for retrieving sales</param>
        /// <returns>List of sales matching the filters</returns>
        /// <response code="200">Sales retrieved successfully</response>
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
        /// <param name="command">Details of the sale to be deleted</param>
        /// <returns>Response indicating the result of the delete operation</returns>
        /// <response code="200">Sale deleted successfully</response>
        /// <response code="404">Sale not found</response>
        [HttpDelete]
        [Consumes("application/json")]
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