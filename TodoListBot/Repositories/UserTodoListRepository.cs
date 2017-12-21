using System;
using System.Collections.Generic;
using System.Linq;
using TodoListBot.Models;

namespace TodoListBot.Repositories
{
    [Serializable]
    public class UserTodoListRepository
    {
        private static List<UserTodoList> UserTodoLists = new List<UserTodoList>();

        public UserTodoList GetUserTodoListByUserId(string userId)
        {
            return UserTodoLists.FirstOrDefault(x => x.UserId == userId);
        }

        public UserTodoList CreateTodo(CreateTaskModel createTaskModel, string userId, string userName)
        {
            var userTodoList = GetUserTodoListByUserId(userId);
            if (userTodoList == null)
            {
                userTodoList = new UserTodoList
                {
                    Id = Guid.NewGuid(),
                    TodoList = new List<Todo>(),
                    UserId = userId,
                    UserName = userName
                };
                UserTodoLists.Add(userTodoList);
            }
            var todo = new Todo
            {
                Id = Guid.NewGuid(),
                DueDate = createTaskModel.DueDate.GetValueOrDefault(DateTime.UtcNow),
                Description = createTaskModel.Description,
                Status = createTaskModel.Status
            };
            userTodoList.TodoList.Add(todo);

            return userTodoList;
        }
    }
}