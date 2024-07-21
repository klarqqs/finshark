using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
            CreatedOn = stockModel.CreatedOn,
            Comments = stockModel.Comments.Select(c => c
                .ToCommentDto())
                .ToList()
        };
    }
    
    public static Stock ToCreateStockDto(this StockCreateDto stockCreateModel)
    {
        return new Stock
        {
            Symbol = stockCreateModel.Symbol,
            CompanyName = stockCreateModel.CompanyName,
            Purchase = stockCreateModel.Purchase,
            LastDiv = stockCreateModel.LastDiv,
            Industry = stockCreateModel.Industry,
            MarketCap = stockCreateModel.MarketCap,
        };
    }
}