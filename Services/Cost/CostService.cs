using Planify_BackEnd.DTOs.CostBreakdown;
using Planify_BackEnd.Models;

public class CostService : ICostService
{
    private readonly ICostRepository _costRepository;

    public CostService(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<CostBreakdownDTO> CreateCostAsync(CostBreakdownDTO costDto)
    {
        var cost = new CostBreakdown
        {
            Name = costDto.Name,
            Quantity = costDto.Quantity,
            PriceByOne = costDto.PriceByOne,
            EventId = costDto.EventId
        };

        var createdCost = await _costRepository.CreateCostAsync(cost);
        return new CostBreakdownDTO
        {
            Id = createdCost.Id,
            Name = createdCost.Name,
            Quantity = createdCost.Quantity,
            PriceByOne = createdCost.PriceByOne,
            EventId = createdCost.EventId
        };
    }

    public async Task<CostBreakdownDTO> UpdateCostAsync(CostBreakdownDTO costDto)
    {
        var existingCost = await _costRepository.GetCostByIdAsync(costDto.Id);
        if (existingCost == null)
        {
            throw new Exception($"Cost with ID {costDto.Id} not found");
        }

        existingCost.Name = costDto.Name;
        existingCost.Quantity = costDto.Quantity;
        existingCost.PriceByOne = costDto.PriceByOne;
        existingCost.EventId = costDto.EventId;

        var updatedCost = await _costRepository.UpdateCostAsync(existingCost);
        return new CostBreakdownDTO
        {
            Id = updatedCost.Id,
            Name = updatedCost.Name,
            Quantity = updatedCost.Quantity,
            PriceByOne = updatedCost.PriceByOne,
            EventId = updatedCost.EventId
        };
    }

    public async System.Threading.Tasks.Task DeleteCostAsync(int id)
    {
        var existingCost = await _costRepository.GetCostByIdAsync(id);
        if (existingCost == null)
        {
            throw new Exception($"Cost with ID {id} not found");
        }

        await _costRepository.DeleteCostAsync(id);
    }
}