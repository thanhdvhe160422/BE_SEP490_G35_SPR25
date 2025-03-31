using Planify_BackEnd.DTOs;
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

    public async Task<ResponseDTO> CreateRiskAsync(RiskCreateDTO riskDto)
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
        return new ResponseDTO(201,"Create Risk Successfully",createdRisk);
    }

    public async Task<ResponseDTO> UpdateRiskAsync(RiskUpdateDTO riskDto)
    {
        var existingRisk = await _riskRepository.GetRiskByIdAsync(riskDto.Id);
        if (existingRisk == null)
        {
            return new ResponseDTO(404, $"Risk with ID {riskDto.Id} not found", null);
        }

        existingRisk.Name = riskDto.Name;
        existingRisk.Reason = riskDto.Reason;
        existingRisk.Solution = riskDto.Solution;
        existingRisk.Description = riskDto.Description;

        var updatedRisk = await _riskRepository.UpdateRiskAsync(existingRisk);
        return new ResponseDTO(200, "Update Rish Successfully", updatedRisk);
    }

    public async Task<ResponseDTO> DeleteRiskAsync(int id)
    {
        var existingRisk = await _riskRepository.GetRiskByIdAsync(id);
        if (existingRisk == null)
        {
            return new ResponseDTO(404, $"Risk with ID {id} not found", null);
        }

        await _riskRepository.DeleteRiskAsync(id);
        return new ResponseDTO(200, "Delete Rish Successfully", null);
    }
}