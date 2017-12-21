using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TodoListBot.Models;
using TodoListBot.Repositories;

namespace TodoListBot.Dialogs
{
    [LuisModel("ModelId", "SubscriptionKey")]
    [Serializable]
    public class RootDialog : LuisDialog<CreateTaskModel>
    {
        private UserTodoListRepository _userTodoListRepository;
        private readonly BuildFormDelegate<CreateTaskModel> CreateTaskDelegate;

        public RootDialog(BuildFormDelegate<CreateTaskModel> createTaskDelegate)
        {
            CreateTaskDelegate = createTaskDelegate;
            _userTodoListRepository = new UserTodoListRepository();
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I don't understand what you mean.");

            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            var activity = context.Activity;
            var userId = activity.From.Id;
            var userName = activity.From.Name;
            var userTodoList = _userTodoListRepository.GetUserTodoListByUserId(userId);
            var reply = context.MakeMessage();
            reply.Text = $"Hi {userName}, how can I help you?";
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;

            reply.SuggestedActions = new SuggestedActions
            {
                Actions = new List<CardAction>
                        {
                            new CardAction { Title = "View my Tasks", Type=ActionTypes.ImBack, Value="View my Tasks" },
                            new CardAction { Title = "Create a Task", Type=ActionTypes.ImBack, Value="Create a Task" }
                        }
            };

            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        [LuisIntent("CreateTask")]
        public Task CreateTask(IDialogContext context, LuisResult result)
        {
            var enrollmentForm = new FormDialog<CreateTaskModel>(new CreateTaskModel(), CreateTaskDelegate, FormOptions.PromptInStart);
            context.Call(enrollmentForm, ResumeAfterCreateTask);

            return Task.CompletedTask;
        }

        [LuisIntent("ViewTasks")]
        public async Task ViewTasks(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("View my tasks not available.");

            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterCreateTask(IDialogContext context, IAwaitable<CreateTaskModel> result)
        {
            try
            {
                var resultFromCreateTask = await result;
                await context.PostAsync($"Creating this task: {resultFromCreateTask}");
                var userTodoList = _userTodoListRepository.CreateTodo(resultFromCreateTask, context.Activity.From.Id, context.Activity.From.Name);
                await context.PostAsync($"Task created, you have now {userTodoList.TodoList.Count} todo(s).");

            }
            catch (FormCanceledException<CreateTaskModel> exception)
            {
                string reply;

                if (exception.InnerException == null)
                {
                    reply = "You have canceled the operation. What would you like to do next?";
                }
                else
                {
                    //TODO log inner exception here
                    reply = $"Internal error, please contact administrator.";
                }

                await context.PostAsync(reply);
            }

            // Again, wait for the next message from the user.
            context.Wait(MessageReceived);
        }
    }
}