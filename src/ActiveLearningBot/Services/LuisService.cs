using Cognitive.LUIS.Programmatic;
using Cognitive.LUIS.Programmatic.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveLearningBot.Services
{
    public static class LuisService
    {
        public static async Task<(string intent, double score)> ExecuteLuisQuery(IConfiguration configuration, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            try
            {
                var luisApplication = new LuisApplication(
                    configuration["LUIS:AppId"],
                    configuration["LUIS:AuthoringKey"],
                    "https://" + configuration["LUIS:APIHostName"]
                );

                var recognizer = new LuisRecognizer(luisApplication);
                var recognizerResult = await recognizer.RecognizeAsync(turnContext, cancellationToken);
                return recognizerResult.GetTopScoringIntent();
            }
            catch (Exception e) { }
            return ("None", 100);
        }

        public static async Task Learn(IConfiguration configuration, string message, string intent)
        {
            using (var client = new LuisProgClient(configuration["LUIS:AuthoringKey"], Regions.WestUS))
            {
                var app = await client.GetAppByNameAsync("Hotel");
                await client.AddExampleAsync(app.Id, app.Endpoints.Production.VersionId, new Example
                {
                    Text = message,
                    IntentName = intent
                });

                var trainingDetails = await client.TrainAndGetFinalStatusAsync(app.Id, app.Endpoints.Production.VersionId);
                if (trainingDetails.Status.Equals("Success"))
                {
                    await client.PublishAsync(app.Id, app.Endpoints.Production.VersionId, false, "westus");
                    await client.PublishAsync(app.Id, app.Endpoints.Production.VersionId, false, "westcentralus");
                }
            }
        }
    }
}
