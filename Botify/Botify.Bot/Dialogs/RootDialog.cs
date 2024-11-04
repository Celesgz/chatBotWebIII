using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Botify.Logica;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples
{
    public class RootDialog : ComponentDialog
    {
        private readonly IBotLogica _botLogica;
        private readonly IStatePropertyAccessor<JObject> _userStateAccessor;

        public RootDialog(UserState userState, IBotLogica botLogica)
            : base("root")
        {
            _userStateAccessor = userState.CreateProperty<JObject>("result");
            _botLogica = botLogica;

            AddDialog(new TextPrompt("namePrompt"));
            AddDialog(new TextPrompt("moodPrompt"));

            AddDialog(new WaterfallDialog("waterfall", new WaterfallStep[]
            {
            AskNameAsync,
            AskMoodAsync,
            ProvideSongsRecommendationsAsync
            }));

            AddDialog(new WaterfallDialog("infinito", new WaterfallStep[]
            {
            AskMoodAgainAsync,
            ProvideSongsRecommendationsAsync
            }));

            InitialDialogId = "waterfall";
        }

        private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("namePrompt", new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor, antes de comenzar, ingresá tu nombre")
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskMoodAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var name = (string)stepContext.Result;
            return await stepContext.PromptAsync("moodPrompt", new PromptOptions
            {
                Prompt = MessageFactory.Text($"Buenas {name}, te saluda Botify! (˶ᵔ ᵕ ᵔ˶) Soy un asistente virtual que te va a ayudar a encontrar las mejores recomendaciones musicales en base a tu estado de ánimo.\n\n" +
                "Por favor, ingresa tu estado de ánimo incluyendo una de las siguientes palabras clave:\n" +
                "- Feliz\n" +
                "- Triste\n" +
                "- Energetico\n" +
                "- Relajado\n" +
                "- Romantico\n" +
                "- Nostalgico\n" +
                "- Furioso\n" +
                "- Optimista\n" +
                "- Melancolico\n" +
                "- Inspirado\n" +
                "- Emocionado\n" +
                "- Aventurero\n" +
                "- Estresado\n" +
                "- Motivador\n")

            }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProvideSongsRecommendationsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mood = (string)stepContext.Result;
            var recommendationsMessage = await _botLogica.ObtenerRecomendaciones(mood);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(recommendationsMessage), cancellationToken);

            return await stepContext.ReplaceDialogAsync("infinito", null, cancellationToken);
        }

        private async Task<DialogTurnResult> AskMoodAgainAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var name = (string)stepContext.Result;
            return await stepContext.PromptAsync("moodPrompt", new PromptOptions
            {
                Prompt = MessageFactory.Text($"Adelante, puedes seguir contándome tu estado de ánimo y encontraré más canciones para ti. ᕙ( •̀ ᗜ •́ )ᕗ")
            }, cancellationToken);
        }
    }

}



