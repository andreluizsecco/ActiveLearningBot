using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLearningBot.Dialogs
{
    public class LoopDialog : ComponentDialog
    {
        public LoopDialog()
            : base(nameof(LoopDialog))
        {
            var steps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), steps));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Deseja algo mais?"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Sim", "Não" }),
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            var loop = ((FoundChoice)stepContext.Result).Value.Equals("Sim");

            if (loop)
                return await stepContext.ReplaceDialogAsync(nameof(RootDialog), null, cancellationToken);
            else
            {
                await stepContext.Context.SendActivityAsync($"Obrigado por utilizar nossos serviços! Até mais.", cancellationToken: cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }


        }
    }
}
