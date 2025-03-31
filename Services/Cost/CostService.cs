using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.CostBreakdown;
using Planify_BackEnd.Models;

public class CostService : ICostService
{
    private readonly ICostRepository _costRepository;

    public CostService(ICostRepository costRepository)
    {
        _costRepository = costRepository;
    }

    public async Task<ResponseDTO> CreateCostAsync(CostBreakdownCreateDTO costDto)
    {
        var cost = new CostBreakdown
        {
            Name = costDto.Name,
            Quantity = costDto.Quantity,
            PriceByOne = costDto.PriceByOne,
            EventId = costDto.EventId
        };

        var createdCost = await _costRepository.CreateCostAsync(cost);
        return new ResponseDTO(201, "Create Cost Successfully", createdCost);
    }

    public async Task<ResponseDTO> UpdateCostAsync(CostBreakdownUpdateDTO costDto)
    {
        var existingCost = await _costRepository.GetCostByIdAsync(costDto.Id);
        if (existingCost == null)
        {
            return new ResponseDTO(404, $"Cost with ID {costDto.Id} not found", null);
        }

        existingCost.Name = costDto.Name;
        existingCost.Quantity = costDto.Quantity;
        existingCost.PriceByOne = costDto.PriceByOne;

        var updatedCost = await _costRepository.UpdateCostAsync(existingCost);
        return new ResponseDTO(200, "Update Cost Successfully", updatedCost);
    }

    public async Task<ResponseDTO> DeleteCostAsync(int id)
    {
        var existingCost = await _costRepository.GetCostByIdAsync(id);
        if (existingCost == null)
        {
            return new ResponseDTO(404, $"Cost with ID {id} not found", null);
        }

        await _costRepository.DeleteCostAsync(id);
        return new ResponseDTO(200, "Delete Cost Successfully", null);
    }
}