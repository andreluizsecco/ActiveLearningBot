using ActiveLearningBot.Extensions;
using ActiveLearningBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLearningBot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private readonly IConfiguration _configuration;
        private readonly UserState _userState;

        public RootDialog(IConfiguration configuration,
                          UserState userState)
            : base(nameof(RootDialog))
        {
            _configuration = configuration;
            _userState = userState;

            var steps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), steps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new LearningDialog(_configuration, _userState));
            AddDialog(new LoopDialog());

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            await stepContext.Context.SendTypingActivity(cancellationToken);
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("O que você deseja?"),
            };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            var accessor = _userState.CreateProperty<string>("Solicitacao");
            await accessor.SetAsync(stepContext.Context, (string)stepContext.Result, cancellationToken);

            var luisResult = await LuisService.ExecuteLuisQuery(_configuration, stepContext.Context, cancellationToken);

            if (luisResult.intent.Equals("None"))
                return await stepContext.BeginDialogAsync(nameof(LearningDialog), null, cancellationToken);
            else
                await stepContext.Context.SendActivityAsync(Conversation.Answer(luisResult.intent), cancellationToken: cancellationToken);

            return await stepContext.ReplaceDialogAsync(nameof(LoopDialog), cancellationToken: cancellationToken);
        }
    }
}
