using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Models;
using Planify_BackEnd.Services.ChatGPT;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventSuggestionController : ControllerBase
    {
        private readonly IChatGPTService _chatGPTService;
        private readonly PlanifyContext _dbContext;

        public EventSuggestionController(IChatGPTService chatGPTService, PlanifyContext dbContext)
        {
            _chatGPTService = chatGPTService;
            _dbContext = dbContext;
        }

        [HttpPost("get-full-event-suggestion")]
        public async Task<IActionResult> GetFullEventSuggestion([FromBody] SuggestionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { Error = "Prompt không được để trống." });
            }

            try
            {
                var pastEvents = await _dbContext.Events
                    .Where(e => e.Status == 1 &&
                               (!request.CategoryEventId.HasValue || e.CategoryEventId == request.CategoryEventId))
                    .Select(e => new
                    {
                        e.EventTitle,
                        e.EventDescription,
                        e.StartTime,
                        e.EndTime,
                        e.CategoryEventId,
                        e.AmountBudget,
                        DurationHours = (e.EndTime - e.StartTime).TotalHours,
                        Tasks = _dbContext.Tasks.Where(t => t.EventId == e.Id)
                            .Select(t => new
                            {
                                t.TaskName,
                                t.TaskDescription,
                                t.StartTime,
                                t.Deadline,
                                t.AmountBudget,
                                t.Status,
                                SubTasks = _dbContext.SubTasks.Where(st => st.TaskId == t.Id)
                                    .Select(st => new
                                    {
                                        st.SubTaskName,
                                        st.SubTaskDescription,
                                        st.StartTime,
                                        st.Deadline,
                                        st.AmountBudget,
                                        st.Status
                                    }).ToList()
                            }).ToList()
                    })
                    .Take(5)
                    .ToListAsync();

                string fullPrompt;

                if (!pastEvents.Any())
                {
                    // Nếu không có dữ liệu lịch sử, tạo prompt để sinh sự kiện mới
                    fullPrompt = $"Không có dữ liệu lịch sử. Hãy tạo một sự kiện mới thực tế và phù hợp cho trường Đại học FPT.\n" +
                                $"Yêu cầu: {request.Prompt}\n" +
                                $"Trả về kết quả bằng tiếng Việt, thời gian phải luôn ở tương lai, ngân sách chuẩn thực tế và dưới dạng JSON chỉ với các trường: " +
                                $"EventTitle (string), EventDescription (string), StartTime (string, định dạng 'yyyy-MM-dd HH:mm'), " +
                                $"EndTime (string, định dạng 'yyyy-MM-dd HH:mm'), CategoryEventId (int) = {request.CategoryEventId}, AmountBudget (decimal), " +
                                $"Tasks (mảng chứa TaskName (string), TaskDescription (string), StartTime (string), Deadline (string), " +
                                $"AmountBudget (decimal), SubTasks (mảng chứa SubTaskName (string), SubTaskDescription (string), " +
                                $"StartTime (string), Deadline (string), AmountBudget (decimal))). " +
                                $"Phản hồi phải cung cấp chi tiết đầy đủ và mô tả kỹ lưỡng kế hoạch. " +
                                $"Đây là sự kiện tổ chức cho trường Đại học FPT.";
                }
                else
                {
                    // Nếu có dữ liệu lịch sử, giữ nguyên logic cũ
                    string eventData = "Dữ liệu lịch sử:\n";
                    foreach (var ev in pastEvents)
                    {
                        eventData += $"- Sự kiện: {ev.EventTitle}\n";
                        eventData += $"  Mô tả: {ev.EventDescription}\n";
                        eventData += $"  Thời gian: {ev.StartTime} - {ev.EndTime} ({ev.DurationHours:F2} giờ)\n";
                        eventData += $"  Thể loại: {ev.CategoryEventId}, Ngân sách: {ev.AmountBudget}\n";
                        eventData += "  Nhiệm vụ:\n";
                        foreach (var task in ev.Tasks)
                        {
                            eventData += $"    - {task.TaskName}: {task.TaskDescription}, Ngân sách: {task.AmountBudget}, Thời gian: {task.StartTime} - {task.Deadline}\n";
                            eventData += "      SubTasks:\n";
                            foreach (var subTask in task.SubTasks)
                            {
                                eventData += $"        - {subTask.SubTaskName}: {subTask.SubTaskDescription}, Ngân sách: {subTask.AmountBudget}, Thời gian: {subTask.StartTime} - {subTask.Deadline}\n";
                            }
                        }
                        eventData += "\n";
                    }

                    fullPrompt = $"{eventData}\nYêu cầu: {request.Prompt}\nTrả về kết quả bằng tiếng Việt, thời gian phải luôn ở tương lai và dưới dạng JSON chỉ với các trường: EventTitle (string), EventDescription (string), StartTime (string, định dạng 'yyyy-MM-dd HH:mm'), EndTime (string, định dạng 'yyyy-MM-dd HH:mm'), CategoryEventId (int), AmountBudget (decimal), Tasks (mảng chứa TaskName (string), TaskDescription (string), StartTime (string), Deadline (string), AmountBudget (decimal), SubTasks (mảng chứa SubTaskName (string), SubTaskDescription (string), StartTime (string), Deadline (string), AmountBudget (decimal))). Phản hồi phải cung cấp chi tiết đầy đủ và mô tả kỹ lưỡng kế hoạch. Đây là sự kiện tổ chức cho trường Đại học FPT.";
                }

                string suggestion = await _chatGPTService.GetSuggestion(fullPrompt);

                try
                {
                    var jsonSuggestion = JsonDocument.Parse(suggestion).RootElement;
                    return Ok(jsonSuggestion);
                }
                catch (JsonException)
                {
                    return Ok(new { Suggestion = suggestion });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        public class SuggestionRequest
        {
            public string Prompt { get; set; }
            public int? CategoryEventId { get; set; }
        }
    }
}