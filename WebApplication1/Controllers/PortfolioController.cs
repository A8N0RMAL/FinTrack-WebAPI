using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Extensions;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
                                   IStockRepository stockRepository,
                                   IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        // 1. get user. 2. get stock. 3. create an obj. 4. save to db.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddStockToPortfolio([FromBody] string stockSymbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username); // 1
            var stock = await _stockRepository.GetStockBySymbolAsync(stockSymbol); // 2
            if (stock == null)
                return NotFound("Stock not found.");
            
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser); 
            if(userPortfolio.Any(s => s.Symbol.ToLower() == stockSymbol.ToLower()))
                return BadRequest("Stock already in portfolio.");

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepository.CreatePortfolioAsync(portfolioModel); // 3
            if(portfolioModel == null)
                return StatusCode(500, "Could not create.");
            else
                return CreatedAtAction(nameof(GetUserPortfolio), new { }, "Stock added to portfolio.");
        }

        // 1. get user. 2. get portfolio. 3. filter. 4. delete.
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio([FromBody] string stockSymbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username); // 1

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser); // 2
            var stockExists = userPortfolio.Any(s => s.Symbol.ToLower() == stockSymbol.ToLower()); // 3

            if (stockExists)
            {
                await _portfolioRepository.DeletePortfolioAsync(appUser, stockSymbol); // 4
                return Ok("Stock removed from portfolio.");
            }
            else
                return BadRequest("Stock is not in your portfolio!");
        }
    }
}
