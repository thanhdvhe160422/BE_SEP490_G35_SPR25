
﻿using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Groups;

namespace Planify_BackEnd.Services.Groups
{
    public interface IGroupService
    {
        Task<ResponseDTO> CreateGroupAsync(GroupCreateRequestDTO groupDTO, Guid organizerId);
        bool AllocateCostToGroup(int groupId, decimal cost);
        bool AddLeadGroup(int GroupId, Guid ImplementerId);
        bool RemoveLeadGroup(int GroupId, Guid ImplementerId);
    }
}
