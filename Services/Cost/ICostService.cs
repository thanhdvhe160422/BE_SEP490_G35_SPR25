using Planify_BackEnd.DTOs.CostBreakdown;

public interface ICostService
{
    Task<CostBreakdownDTO> CreateCostAsync(CostBreakdownDTO costDto);
    Task<CostBreakdownDTO> UpdateCostAsync(CostBreakdownDTO costDto);
    Task DeleteCostAsync(int id);
}