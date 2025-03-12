namespace Planify_BackEnd.DTOs.SendRequests
{
    public class SendRequestDTO
    {
        public int EventId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
