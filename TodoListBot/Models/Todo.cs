using System;

namespace TodoListBot.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TodoStatus? Status { get; set; }
    }
}
