using Microsoft.Bot.Builder.FormFlow;
using System;

namespace TodoListBot.Models
{
    [Serializable]
    public class CreateTaskModel
    {
        public string Description;
        public TodoStatus? Status;
        public DateTime? DueDate;

        public static IForm<CreateTaskModel> BuildForm()
        {
            return new FormBuilder<CreateTaskModel>()
                .Message("Welcome to the task creation bot!")
                .Build();
        }

        public override string ToString() => $"Description '{Description}', status '{Status}', Due Date {DueDate}";
    }
}