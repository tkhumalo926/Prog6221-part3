using System;

namespace Prog_6221_Part3_PoE.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; }
    }
}
