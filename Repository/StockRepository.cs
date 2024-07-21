using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository(ApplicationDBContext context) : IStockRepository
{
    public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
    {
        var stocks = context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace((query.Symbol)))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }

            if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
            }
            
            if (query.SortBy.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.Purchase) : stocks.OrderBy(s => s.Purchase);
            }

            if (query.SortBy.Equals("LastDiv", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.LastDiv) : stocks.OrderBy(s => s.LastDiv);
            }
            
            if (query.SortBy.Equals("Industry", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.Industry) : stocks.OrderBy(s => s.Industry);
            }

            if (query.SortBy.Equals("MarketCap", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.MarketCap) : stocks.OrderBy(s => s.MarketCap);
            }
            
            if (query.SortBy.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.isDescending ? stocks.OrderByDescending(s => s.CreatedOn) : stocks.OrderBy(s => s.CreatedOn);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetStockByIdAsync(Guid id)
    {
        return await context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        return await context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> CreateStockAsync(Stock stock)
    {
        await context.Stocks.AddAsync(stock);
        await context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateStockAsync(Guid id, StockUpdateDto stockUpdateDto)
    {
        var existingStock = await context.Stocks.FindAsync(id);
        if (existingStock is null)
            return null;
        existingStock.Symbol = stockUpdateDto.Symbol;
        existingStock.CompanyName = stockUpdateDto.CompanyName;
        existingStock.Purchase = stockUpdateDto.Purchase;
        existingStock.Industry = stockUpdateDto.Industry;
        existingStock.LastDiv = stockUpdateDto.LastDiv;
        existingStock.MarketCap = stockUpdateDto.MarketCap;
        await context.SaveChangesAsync();
        return existingStock;
    }

    public async Task<Stock?> DeleteStockAsync(Guid id)
    {
        var stock = await context.Stocks.FindAsync(id);
        if (stock is null)
            return null;
        context.Stocks.Remove(stock);
        await context.SaveChangesAsync();
        return stock;
    }

    public Task<bool> StockExists(Guid id)
    {
        return context.Stocks.AnyAsync(s => s.Id == id);
    }
}
