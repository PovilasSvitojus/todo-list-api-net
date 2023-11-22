using System;
using System.Collections.Generic;

namespace TodoList.Models
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string TaskDesc { get; set; } = null!;
        public string TaskStatus { get; set; } = null!;
    }
}
