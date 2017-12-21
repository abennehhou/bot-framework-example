using System;
using System.Collections.Generic;

namespace TodoListBot.Models
{
    public class UserTodoList
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<Todo> TodoList { get; set; }
    }
}