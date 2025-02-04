using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository(ApplicationDBContext context) : IPortfolioRepository
{
    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        var userId = user.Id;

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(user.Id), "User ID cannot be null");
        }
        
        var portfolios = await context.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => new Stock
        {
            Id = stock.StockId,
            Symbol = stock.Stock.Symbol,
            CompanyName = stock.Stock.CompanyName,
            Purchase = stock.Stock.Purchase,
            LastDiv = stock.Stock.LastDiv,
            Industry = stock.Stock.Industry,
            MarketCap = stock.Stock.MarketCap,

        }).ToListAsync();

        return portfolios;
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await context.Portfolios.AddAsync(portfolio);
        await context.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
    {
        var portfolioModel = await context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null)
        {
            return null;
        }

        context.Portfolios.Remove(portfolioModel);
        await context.SaveChangesAsync();
        return portfolioModel;
    }
}