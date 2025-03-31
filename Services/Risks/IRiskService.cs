using Planify_BackEnd.DTOs.Risk;

public interface IRiskService
{
    Task<RiskDTO> CreateRiskAsync(RiskDTO riskDto);
    Task<RiskDTO> UpdateRiskAsync(RiskDTO riskDto);
    Task DeleteRiskAsync(int id);
}