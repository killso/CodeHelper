namespace Codehelper.WebServer
{
    public class Statistic
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public string? ErrorCode { get; set; }

        public float SuccessRate { get; set; }
    }
}