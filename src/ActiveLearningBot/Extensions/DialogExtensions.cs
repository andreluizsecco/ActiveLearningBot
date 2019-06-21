using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLearningBot.Extensions
{
    public static class DialogExtensions
    {
        public static async Task Run(this Dialog dialog, ITurnContext turnContext, IStatePropertyAccessor<DialogState> accessor, CancellationToken cancellationToken = default)
        {
            var dialogSet = new DialogSet(accessor);
            dialogSet.Add(dialog);

            var dialogContext = await dialogSet.CreateContextAsync(turnContext, cancellationToken);
            var results = await dialogContext.ContinueDialogAsync(cancellationToken);
            if (results.Status == DialogTurnStatus.Empty)
            {
                await dialogContext.BeginDialogAsync(dialog.Id, null, cancellationToken);
            }
        }

        public static async Task<ResourceResponse> SendTypingActivity(this ITurnContext context, CancellationToken cancellationToken = default)
        {
            var activity = Activity.CreateEventActivity();
            activity.Type = ActivityTypes.Typing;
            return await context.SendActivityAsync(activity, cancellationToken: cancellationToken);
        }
    }
}
