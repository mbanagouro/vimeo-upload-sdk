using System;

namespace VimeoAPI
{
    /// <summary>
    /// Ticket do
    /// </summary>
    public class VimeoTicket
    {
        public string EndPoint { get; set; }

        public string FilePath { get; set; }

        public int FileSize { get; set; }

        public int MaxUpload { get; set; }

        public int SpaceFree { get; set; }

        public int SpaceUsed { get; set; }

        public string TicketId { get; set; }

        public int UserId { get; set; }

        public string VideoId { get; set; }
    }
}
