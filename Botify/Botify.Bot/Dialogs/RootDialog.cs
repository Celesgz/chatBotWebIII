// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
    //public class RootDialog : ComponentDialog
    //{
    //    private readonly IBotLogica _botLogica;
    //    private readonly IStatePropertyAccessor<JObject> _userStateAccessor;

    //    public RootDialog(UserState userState, IBotLogica botLogica)
    //        : base("root")
    //    {
    //        _userStateAccessor = userState.CreateProperty<JObject>("result");
    //        _botLogica = botLogica;

    //        // Add the various dialogs that will be used to the DialogSet.
    //        AddDialog(new TextPrompt("namePrompt"));
    //        AddDialog(new TextPrompt("moodPrompt"));

    //        // Defines a simple two step Waterfall to test the slot dialog.
    //        AddDialog(new WaterfallDialog("waterfall", new WaterfallStep[] { AskNameAsync, AskMoodAsync, ProvideSongsRecommendationsAsync }));

    //        // The initial child Dialog to run.
    //        InitialDialogId = "waterfall";
    //    }


    //    // 1- pide nombre
    //    private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //    {
    //        return await stepContext.PromptAsync("namePrompt", new PromptOptions { Prompt = MessageFactory.Text("Por favor, antes de comenzar, ingresá tu nombre") }, cancellationToken);
    //    }

    //    // eleccion canciones - pregunta por estado
    //    private async Task<DialogTurnResult> AskMoodAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //    {
    //        var name = (string)stepContext.Result;
    //        return await stepContext.PromptAsync("moodPrompt", new PromptOptions { Prompt = MessageFactory.Text($"Buenas {name}, te saluda Botify! (˶ᵔ ᵕ ᵔ˶) Soy un asistente virtual que te va a ayudar a encontrar las mejores recomendaciones musicales en base a tu estado de animo.\n\nPara darte canciones, dime directamente como te sientes, es importante que ingreses un estado directo (feliz, triste, entusisamado, etc) sin roscas nos entendemos mejor, no? ") }, cancellationToken);
    //    }

    //    // eleccion canciones - trae las canciones por el animo ingresado
    //    private async Task<DialogTurnResult> ProvideSongsRecommendationsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //    {
    //        var mood = (string)stepContext.Result;
    //        var recommendationsMessage = await _botLogica.ObtenerRecomendaciones(mood);
    //        await stepContext.Context.SendActivityAsync(MessageFactory.Text(recommendationsMessage), cancellationToken);
    //        return await stepContext.EndDialogAsync(null, cancellationToken);
    //    }

    //}

    public class RootDialog : ComponentDialog
    {
        private readonly IBotLogica _botLogica;
        private readonly IStatePropertyAccessor<JObject> _userStateAccessor;

        public RootDialog(UserState userState, IBotLogica botLogica)
            : base("root")
        {
            _userStateAccessor = userState.CreateProperty<JObject>("result");
            _botLogica = botLogica;

            // Add the various dialogs that will be used to the DialogSet.
            AddDialog(new TextPrompt("namePrompt"));
            AddDialog(new TextPrompt("moodPrompt"));

            // Define a simple WaterfallDialog with multiple steps.
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

            // The initial child Dialog to run.
            InitialDialogId = "waterfall";
        }

        // 1- Pide nombre
        private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("namePrompt", new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor, antes de comenzar, ingresá tu nombre")
            }, cancellationToken);
        }

        // 2- Elección de canciones - pregunta por estado
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
                "- Motivador\n")              

            }, cancellationToken);
        }

        // 3- Elección de canciones - trae las canciones por el ánimo ingresado
        private async Task<DialogTurnResult> ProvideSongsRecommendationsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var mood = (string)stepContext.Result;
            var recommendationsMessage = await _botLogica.ObtenerRecomendaciones(mood);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(recommendationsMessage), cancellationToken);

            // Pregunta por otro estado de ánimo
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



