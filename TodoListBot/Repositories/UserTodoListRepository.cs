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
    }
}