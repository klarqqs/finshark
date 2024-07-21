using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllStocksAsync(QueryObject query);
    Task<Stock?> GetStockByIdAsync(Guid id);
    Task<Stock?> GetStockBySymbolAsync(string symbol);
    Task<Stock> CreateStockAsync(Stock stock);
    Task<Stock?> UpdateStockAsync(Guid id, StockUpdateDto stockUpdateDto);
    Task<Stock?> DeleteStockAsync(Guid id);
    Task<bool> StockExists(Guid id);
}