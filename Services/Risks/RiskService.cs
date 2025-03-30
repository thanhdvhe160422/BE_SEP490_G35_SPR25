using Planify_BackEnd.DTOs.Risk;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Risk;

public class RiskService : IRiskService
{
    private readonly IRiskRepository _riskRepository;

    public RiskService(IRiskRepository riskRepository)
    {
        _riskRepository = riskRepository;
    }

    public async Task<RiskDTO> CreateRiskAsync(RiskDTO riskDto)
    {
        var risk = new Risk
        {
            EventId = riskDto.EventId,
            Name = riskDto.Name,
            Reason = riskDto.Reason,
            Solution = riskDto.Solution,
            Description = riskDto.Description
        };

        var createdRisk = await _riskRepository.CreateRiskAsync(risk);
        return new RiskDTO
        {
            Id = createdRisk.Id,
            EventId = createdRisk.EventId,
            Name = createdRisk.Name,
            Reason = createdRisk.Reason,
            Solution = createdRisk.Solution,
            Description = createdRisk.Description
        };
    }

    public async Task<RiskDTO> UpdateRiskAsync(RiskDTO riskDto)
    {
        var existingRisk = await _riskRepository.GetRiskByIdAsync(riskDto.Id);
        if (existingRisk == null)
        {
            throw new Exception($"Risk with ID {riskDto.Id} not found");
        }

        existingRisk.Name = riskDto.Name;
        existingRisk.Reason = riskDto.Reason;
        existingRisk.Solution = riskDto.Solution;
        existingRisk.Description = riskDto.Description;

        var updatedRisk = await _riskRepository.UpdateRiskAsync(existingRisk);
        return new RiskDTO
        {
            Id = updatedRisk.Id,
            EventId = updatedRisk.EventId,
            Name = updatedRisk.Name,
            Reason = updatedRisk.Reason,
            Solution = updatedRisk.Solution,
            Description = updatedRisk.Description
        };
    }

    public async System.Threading.Tasks.Task DeleteRiskAsync(int id)
    {
        var existingRisk = await _riskRepository.GetRiskByIdAsync(id);
        if (existingRisk == null)
        {
            throw new Exception($"Risk with ID {id} not found");
        }

        await _riskRepository.DeleteRiskAsync(id);
    }
}