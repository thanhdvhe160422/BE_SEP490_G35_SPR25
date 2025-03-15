using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.JoinGroups;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.JoinGroups;

namespace Planify_BackEnd.Services.JoinGroups
{
    public class JoinGroupService : IJoinGroupService
    {
        private readonly IJoinGroupRepository _joinGroupRepository;

        public JoinGroupService(IJoinGroupRepository joinGroupRepository)
        {
            _joinGroupRepository = joinGroupRepository;
        }

        public async Task<ResponseDTO> AddImplementersToGroup(JoinGroupRequestDTO request)
        {
            // Kiểm tra Group có tồn tại không
            var group = await _joinGroupRepository.GetGroupById(request.GroupId);
            if (group == null)
                return new ResponseDTO(400, "Group không tồn tại!", null);

            var failedImplementers = new List<Guid>(); // Danh sách các implementer không thể thêm vào
            var successfulImplementers = new List<Guid>(); // Danh sách các implementer đã thêm thành công

            foreach (var implementerId in request.ImplementerIds)
            {
                // Kiểm tra Implementer có trong Project không
                bool isInProject = await _joinGroupRepository.IsUserInProject(implementerId, group.EventId);
                if (!isInProject)
                {
                    failedImplementers.Add(implementerId);
                    continue;
                }

                // Kiểm tra Implementer đã ở trong Group chưa
                bool isInGroup = await _joinGroupRepository.IsImplementerInGroup(implementerId, request.GroupId);
                if (isInGroup)
                {
                    failedImplementers.Add(implementerId);
                    continue;
                }

                // Nếu tất cả các điều kiện trên đều thỏa mãn, thêm implementer vào danh sách thành công
                successfulImplementers.Add(implementerId);
            }

            // Nếu có implementer nào thành công, tiến hành thêm vào Group
            if (successfulImplementers.Count > 0)
            {
                bool result = await _joinGroupRepository.AddImplementersToGroup(successfulImplementers, request.GroupId);
                if (result)
                {
                    var successMessage = $"Đã thêm {successfulImplementers.Count} Implementers vào Group thành công!";
                    return new ResponseDTO(200, successMessage, successfulImplementers);
                }
            }

            // Nếu không có implementer nào được thêm vào
            if (failedImplementers.Count == request.ImplementerIds.Count)
            {
                return new ResponseDTO(400, "Tất cả Implementers không thể thêm vào Group!", null);
            }

            return new ResponseDTO(500, "Có lỗi xảy ra khi thêm Implementers vào Group!", null);
        }
    }
}
