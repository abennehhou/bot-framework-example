using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TodoListBot.Models;
using TodoListBot.Repositories;

namespace TodoListBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private UserTodoListRepository _userTodoListRepository;

        public RootDialog()
        {
            _userTodoListRepository = new UserTodoListRepository();
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity == null)
                await context.PostAsync("Oups, something wrong occurred :/");
            else
            {
                var text = activity.Text;
                if (text == IntentEnum.ViewTasks.ToString())
                {
                    //TODO Implement ViewTasks
                    await context.PostAsync("View Tasks to be implemented");
                }
                else if (text == IntentEnum.CreateTask.ToString())
                {
                    //TODO Implement ViewTasks
                    await context.PostAsync("Create Task to be implemented");
                }
                else
                {
                    var userId = activity.From.Id;
                    var userName = activity.From.Name;
                    var userTodoList = _userTodoListRepository.GetUserTodoListByUserId(userId);
                    var reply = activity.CreateReply($"Hi {userName}, how can I help you?");
                    reply.Type = ActivityTypes.Message;
                    reply.TextFormat = TextFormatTypes.Plain;

                    reply.SuggestedActions = new SuggestedActions
                    {
                        Actions = new List<CardAction>
                        {
                            new CardAction { Title = "View my Tasks", Type=ActionTypes.ImBack, Value=IntentEnum.ViewTasks.ToString() },
                            new CardAction { Title = "Create a Task", Type=ActionTypes.ImBack, Value=IntentEnum.CreateTask.ToString() }
                        }
                    };

                    await context.PostAsync(reply);
                }
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}