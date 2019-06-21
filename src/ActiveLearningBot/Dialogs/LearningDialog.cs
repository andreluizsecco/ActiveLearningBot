using ActiveLearningBot.Extensions;
using ActiveLearningBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLearningBot.Dialogs
{
    public class LearningDialog : ComponentDialog
    {
        private readonly IConfiguration _configuration;
        private readonly UserState _userState;

        public LearningDialog(IConfiguration configuration,
                              UserState userState)
            : base(nameof(LearningDialog))
        {
            _configuration = configuration;
            _userState = userState;

            var steps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), steps));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new LoopDialog());

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            await stepContext.Context.SendTypingActivity(cancellationToken);
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Ainda estou aprendendo, qual dessas opções representa o que você deseja?"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Fazer uma reserva", "Solicitar o serviço de quarto", "Solicitar o serviço de despertador", "Estou apenas te cumprimentando :)", "Nenhuma", "Sair" }),
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            var accessor = _userState.CreateProperty<string>("Solicitacao");
            var solicitacao = await accessor.GetAsync(stepContext.Context, cancellationToken: cancellationToken);

            var intencaoSolicitacao = ((FoundChoice)stepContext.Result).Value;

            var intentName = "None";

            switch (intencaoSolicitacao)
            {
                case "Nenhuma":
                    intentName = "None";
                    break;
                case "Estou apenas te cumprimentando :)":
                    intentName = "Saudacao";
                    break;
                case "Fazer uma reserva":
                    intentName = "Reserva";
                    break;
                case "Solicitar o serviço de quarto":
                    intentName = "ServicoQuarto";
                    break;
                case "Solicitar o serviço de despertador":
                    intentName = "Despertador";
                    break;
                case "Sair":
                    return await stepContext.ReplaceDialogAsync(nameof(RootDialog), cancellationToken: cancellationToken);
            }
            Task.Run(() => LuisService.Learn(_configuration, solicitacao, intentName));

            var message = Conversation.Answer(intentName);

            await stepContext.Context.SendActivityAsync(message, cancellationToken: cancellationToken);
            return await stepContext.ReplaceDialogAsync(nameof(LoopDialog), cancellationToken: cancellationToken);
        }
    }
}
