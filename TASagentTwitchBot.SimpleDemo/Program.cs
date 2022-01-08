using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

using TASagentTwitchBot.Core.Extensions;
using TASagentTwitchBot.Core.Web;
using TASagentTwitchBot.Plugin.ControllerSpy.Web;
using TASagentTwitchBot.Plugin.TTTAS.Web;

//Initialize DataManagement
BGC.IO.DataManagement.Initialize("TASagentBotDemo");

//
// Define and register services
//

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseKestrel()
    .UseUrls("http://0.0.0.0:5000");

IMvcBuilder mvcBuilder = builder.Services.GetMvcBuilder();

//Register ControllerSpy for inclusion
mvcBuilder.AddControllerSpyControllerAssembly();

//Register TTTAS Assembly for inclusion
mvcBuilder.AddTTTASAssembly();

//Register Core Controllers (with potential exclusions) 
mvcBuilder.RegisterControllersWithoutFeatures(Array.Empty<string>());

//Add SignalR for Hubs
builder.Services.AddSignalR();

//Custom Database
builder.Services
    .AddDbContext<TASagentTwitchBot.SimpleDemo.Database.DatabaseContext>();

//Register custom database to be served for BaseDatabaseContext
builder.Services
    .AddScoped<TASagentTwitchBot.Core.Database.BaseDatabaseContext>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Database.DatabaseContext>());

//Core Agnostic Systems
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Config.BotConfiguration>(TASagentTwitchBot.Core.Config.BotConfiguration.GetConfig())
    .AddSingleton<TASagentTwitchBot.Core.IConfigurator, TASagentTwitchBot.Core.StandardConfigurator>()
    .AddSingleton<TASagentTwitchBot.Core.ICommunication, TASagentTwitchBot.Core.CommunicationHandler>()
    .AddSingleton<TASagentTwitchBot.Core.View.IConsoleOutput, TASagentTwitchBot.Core.View.BasicView>()
    .AddSingleton<TASagentTwitchBot.Core.ErrorHandler>()
    .AddSingleton<TASagentTwitchBot.Core.ApplicationManagement>()
    .AddSingleton<TASagentTwitchBot.Core.Chat.ChatLogger>()
    .AddSingleton<TASagentTwitchBot.Core.IMessageAccumulator, TASagentTwitchBot.Core.MessageAccumulator>();

//Core Twitch Systems
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.API.Twitch.HelixHelper>()
    .AddSingleton<TASagentTwitchBot.Core.API.Twitch.IBotTokenValidator, TASagentTwitchBot.Core.API.Twitch.BotTokenValidator>()
    .AddSingleton<TASagentTwitchBot.Core.API.Twitch.IBroadcasterTokenValidator, TASagentTwitchBot.Core.API.Twitch.BroadcasterTokenValidator>()
    .AddSingleton<TASagentTwitchBot.Core.Database.IUserHelper, TASagentTwitchBot.Core.Database.UserHelper>()
    .AddSingleton<TASagentTwitchBot.Core.Bits.CheerHelper>()
    .AddSingleton<TASagentTwitchBot.Core.Bits.CheerDispatcher>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.TestCommandSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.ShoutOutSystem>();

//Core Twitch Chat Systems
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.IRC.IrcClient>()
    .AddSingleton<TASagentTwitchBot.Core.IRC.IIRCLogger, TASagentTwitchBot.Core.IRC.IRCLogger>()
    .AddSingleton<TASagentTwitchBot.Core.IRC.INoticeHandler, TASagentTwitchBot.Core.IRC.NoticeHandler>()
    .AddSingleton<TASagentTwitchBot.Core.Chat.IChatMessageHandler, TASagentTwitchBot.Core.Chat.ChatMessageHandler>();

//Notification System
//Core Notifications
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Notifications.NotificationServer>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.NotificationSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Notifications.IActivityDispatcher, TASagentTwitchBot.Core.Notifications.ActivityDispatcher>();
//Custom Notification
builder.Services
    .AddSingleton<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Notifications.FullActivityProvider>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.ISubscriptionHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.ICheerHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.IRaidHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.IGiftSubHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.IFollowerHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>())
    .AddSingleton<TASagentTwitchBot.Core.Notifications.ITTSHandler>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.Notifications.CustomActivityProvider>());

//Core Audio System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Audio.IAudioPlayer, TASagentTwitchBot.Core.Audio.AudioPlayer>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.IMicrophoneHandler, TASagentTwitchBot.Core.Audio.MicrophoneHandler>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.ISoundEffectSystem, TASagentTwitchBot.Core.Audio.SoundEffectSystem>();


//Core Audio Effects System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectSystem, TASagentTwitchBot.Core.Audio.Effects.AudioEffectSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.ChorusEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.EchoEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.FrequencyModulationEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.FrequencyShiftEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.NoiseVocoderEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.PitchShiftEffectProvider>()
    .AddSingleton<TASagentTwitchBot.Core.Audio.Effects.IAudioEffectProvider, TASagentTwitchBot.Core.Audio.Effects.ReverbEffectProvider>();

//Core PubSub System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.PubSub.PubSubClient>()
    .AddSingleton<TASagentTwitchBot.Core.PubSub.IRedemptionSystem, TASagentTwitchBot.Core.PubSub.RedemptionSystem>();

//Core Midi System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Audio.MidiKeyboardHandler>();

//Core Emote Effects System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.API.BTTV.BTTVHelper>()
    .AddSingleton<TASagentTwitchBot.Core.EmoteEffects.EmoteEffectConfiguration>(TASagentTwitchBot.Core.EmoteEffects.EmoteEffectConfiguration.GetConfig())
    .AddSingleton<TASagentTwitchBot.Core.EmoteEffects.IEmoteEffectListener, TASagentTwitchBot.Core.EmoteEffects.EmoteEffectListener>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.EmoteEffects.EmoteEffectSystem>();

//Core WebServer Config (Shared by WebTTS and EventSub)
builder.Services
    .AddSingleton(TASagentTwitchBot.Core.Config.ServerConfig.GetConfig());

//Core Local TTS System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.TTS.TTSConfiguration>(TASagentTwitchBot.Core.TTS.TTSConfiguration.GetConfig())
    .AddSingleton<TASagentTwitchBot.Core.TTS.ITTSRenderer, TASagentTwitchBot.Core.TTS.TTSRenderer>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.TTS.TTSSystem>();


//EventSub System
//Core EventSub
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.EventSub.EventSubHandler>()
    .AddSingleton<TASagentTwitchBot.Core.EventSub.IEventSubSubscriber, TASagentTwitchBot.Core.EventSub.FollowSubscriber>()
    .AddSingleton<TASagentTwitchBot.Core.EventSub.IEventSubSubscriber, TASagentTwitchBot.Core.EventSub.StreamChangeSubscriber>();
//Custom EventSub
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.EventSub.IStreamLiveListener, TASagentTwitchBot.SimpleDemo.EventSub.TestLiveListener>();


//Core Timer System
builder.Services.AddSingleton<TASagentTwitchBot.Core.Timer.ITimerManager, TASagentTwitchBot.Core.Timer.TimerManager>();

//Core Controller Overlay
builder.Services.RegisterControllerSpyServices();

//Core TTTAS System
builder.Services.RegisterTTTASServices();

//Command System
//Core Commands
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Commands.CommandSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.CustomCommands>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.SystemCommandSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Commands.PermissionSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Quotes.QuoteSystem>();
//Custom Commands
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.SimpleDemo.Commands.UpTimeSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.SimpleDemo.Commands.TestCommandSystem>();

//Core Credit System
builder.Services
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.Core.Credit.BasicCreditCommandSystem>()
    .AddSingleton<TASagentTwitchBot.Core.Credit.ICreditManager, TASagentTwitchBot.Core.Credit.SimpleCreditManager>();

//Custom Point-spender System
builder.Services
    .AddSingleton<TASagentTwitchBot.SimpleDemo.PointsSpender.IPointSpenderHandler, TASagentTwitchBot.SimpleDemo.PointsSpender.PointSpenderHandler>()
    .AddSingleton<TASagentTwitchBot.Core.Commands.ICommandContainer, TASagentTwitchBot.SimpleDemo.PointsSpender.PointsSpenderSystem>()
    .AddSingleton<TASagentTwitchBot.Core.PubSub.IRedemptionContainer>(x => x.GetRequiredService<TASagentTwitchBot.SimpleDemo.PointsSpender.IPointSpenderHandler>());

//Routing
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});


//
// Finished defining services
// Construct application
//

WebApplication? app = builder.Build();

//Handle forwarding properly
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthorization();
app.UseDefaultFiles();

//Custom Web Assets
app.UseStaticFiles();

//Core Web Assets
app.UseCoreLibraryContent("TASagentTwitchBot.Core");

//Core Controllerspy Assets
app.UseCoreLibraryContent("TASagentTwitchBot.Plugin.ControllerSpy");

//Core TTTAS Assets
app.UseCoreLibraryContent("TASagentTwitchBot.Plugin.TTTAS");

//Authentication Middleware
app.UseMiddleware<TASagentTwitchBot.Core.Web.Middleware.AuthCheckerMiddleware>();

//Map all Core Non-excluded controllers
app.MapControllers();

//Core Notification Hub
app.MapHub<TASagentTwitchBot.Core.Web.Hubs.OverlayHub>("/Hubs/Overlay");

//Core TTS Overlay Hub
app.MapHub<TASagentTwitchBot.Core.Web.Hubs.TTSMarqueeHub>("/Hubs/TTSMarquee");

//Core Control Page Hub
app.MapHub<TASagentTwitchBot.Core.Web.Hubs.MonitorHub>("/Hubs/Monitor");

//Core Timer Hub
app.MapHub<TASagentTwitchBot.Core.Web.Hubs.TimerHub>("/Hubs/Timer");

//Core Emote Effect Overlay Hub
app.MapHub<TASagentTwitchBot.Core.Web.Hubs.EmoteHub>("/Hubs/Emote");

//Core ControllerSpy Endpoints
app.RegisterControllerSpyEndpoints();

//Core TTTAS Endpoints
app.RegisterTTTASEndpoints();


await app.StartAsync();

//
// Update Database with new migrations
//

using (IServiceScope serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope())
{
    TASagentTwitchBot.SimpleDemo.Database.DatabaseContext context = serviceScope.ServiceProvider!.GetRequiredService<TASagentTwitchBot.SimpleDemo.Database.DatabaseContext>();
    context.Database.Migrate();
}

//
// Construct and run Configurator
//


TASagentTwitchBot.Core.ICommunication communication = app.Services.GetRequiredService<TASagentTwitchBot.Core.ICommunication>();
TASagentTwitchBot.Core.IConfigurator configurator = app.Services.GetRequiredService<TASagentTwitchBot.Core.IConfigurator>();

app.Services.GetRequiredService<TASagentTwitchBot.Core.View.IConsoleOutput>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.Chat.ChatLogger>();

bool configurationSuccessful = await configurator.VerifyConfigured();

if (!configurationSuccessful)
{
    communication.SendErrorMessage($"Configuration unsuccessful.  Aborting.");

    await app.StopAsync();

    Environment.Exit(1);
    return;
}

//
// Construct required components and run
//

TASagentTwitchBot.Core.ErrorHandler errorHandler = app.Services.GetRequiredService<TASagentTwitchBot.Core.ErrorHandler>();
TASagentTwitchBot.Core.ApplicationManagement applicationManagement = app.Services.GetRequiredService<TASagentTwitchBot.Core.ApplicationManagement>();

TASagentTwitchBot.Core.API.Twitch.IBotTokenValidator botTokenValidator = app.Services.GetRequiredService<TASagentTwitchBot.Core.API.Twitch.IBotTokenValidator>();
TASagentTwitchBot.Core.API.Twitch.IBroadcasterTokenValidator broadcasterTokenValidator = app.Services.GetRequiredService<TASagentTwitchBot.Core.API.Twitch.IBroadcasterTokenValidator>();

app.Services.GetRequiredService<TASagentTwitchBot.Core.Commands.CommandSystem>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.EventSub.EventSubHandler>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.Bits.CheerDispatcher>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.Audio.IMicrophoneHandler>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.Audio.MidiKeyboardHandler>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.IMessageAccumulator>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.IRC.IrcClient>();
app.Services.GetRequiredService<TASagentTwitchBot.Core.PubSub.PubSubClient>();

try
{
    communication.SendDebugMessage("*** Starting Up ***");

    //Kick off Validators
    botTokenValidator.RunValidator();
    broadcasterTokenValidator.RunValidator();
}
catch (Exception ex)
{
    errorHandler.LogFatalException(ex);
}


//
// Wait for signal to end application
//

try
{
    await applicationManagement.WaitForEndAsync();
}
catch (Exception ex)
{
    errorHandler.LogSystemException(ex);
}

//
// Stop webhost
//

await app.StopAsync();
