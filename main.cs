using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using System.Security;
using System.Threading;
using System.ComponentModel.Design;


Console.Title = "AnonBot - Host";
Console.WriteLine("Paste Bot tokken...");
string token = Console.ReadLine();


var botClient = new TelegramBotClient(token);

using CancellationTokenSource cts = new();


IDictionary<long,string> GlobalR = new Dictionary<long,string>();
Random rand = new Random();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

var me = await botClient.GetMeAsync();
Console.Title = "AnonBot: " + me.Username + " - Host";

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);


Console.WriteLine($"Start listening for @{me.Username}");
Console.WriteLine($"Press Enter To Terminate token...");
Console.ReadLine();

cts.Cancel();

Console.WriteLine("Token Terminated!");

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)//Reciveascync(main func)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Rec: '{messageText}' ;ChatId: {chatId} ;NameOfClient {message.From.Username}");
     
    if (messageText == "/con")
    {
        if (!GlobalR.ContainsKey(chatId))
        {
            Con(chatId);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Connected as " + GlobalR[chatId],
                cancellationToken: cancellationToken);

            foreach (var buff in GlobalR)
            {
                if (buff.Key != chatId)
                {
                    Message sm = await botClient.SendTextMessageAsync(
                    chatId: buff.Key,
                    text: GlobalR[chatId] + " Connected!!!",
                    cancellationToken: cancellationToken);
                }
            }
        }
        else
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You Are Already Connected!!!",
            cancellationToken: cancellationToken);
        }
    }
    else if (messageText == "/dcon")
    {
        if (GlobalR.ContainsKey(chatId))
        {
            DCon(chatId);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Disconected...",
                cancellationToken: cancellationToken);

            foreach (var buff in GlobalR)
            {
                if (buff.Key != chatId)
                {
                    Message sm = await botClient.SendTextMessageAsync(
                    chatId: buff.Key,
                    text: GlobalR[chatId] + " Disconected!!!",
                    cancellationToken: cancellationToken);
                }
            }
        }
        else
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: "Cant Disconnect If You Are Not Connected!!!",
               cancellationToken: cancellationToken);
        }
    } 
    else if (messageText.Length > 6)
    {
        if (messageText.Substring(0,5) == "/name")
        {
            if (GlobalR.ContainsKey(chatId))
            {
                string old = GlobalR[chatId];
                GlobalR[chatId] = messageText.Substring(6);
                Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Name Changed To " + messageText.Substring(6),
                cancellationToken: cancellationToken);

                foreach (var buff in GlobalR)
                {
                    if (buff.Key != chatId)
                    {
                        Message sm = await botClient.SendTextMessageAsync(
                        chatId: buff.Key,
                        text: old + " Changed Name To " + GlobalR[chatId],
                        cancellationToken: cancellationToken);
                    }
                }
            }
            else
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: "You Need To Be Connected To Change Name!",
                       cancellationToken: cancellationToken);
            }
        }
    }
    else if (messageText == "/help")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: me.Username + ": \n/con to Connect\n" +
            "/dcon to Disconnect\n" +
            "/name *name* changes name",
            cancellationToken: cancellationToken);
    }
    else if (messageText.Substring(0, 1) == "/")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: me.Username + ": Command Dont Exist!!!",
            cancellationToken: cancellationToken);
    }
    else if (messageText == "/start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: me.Username + ": To Start Typing With Others Type /con (for help type /help)",
            cancellationToken: cancellationToken);
    }


    if (GlobalR.ContainsKey(chatId) && messageText.Substring(0,1) != "/")
    {
       foreach(var buff in GlobalR) {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: buff.Key,
            text: GlobalR[chatId] + "# " + messageText,
            cancellationToken: cancellationToken);
       }
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) //Error handl
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

void Con(long chatid)//connect user
{
    char[] buff = new char[10];
    
    for(int counter = 0; counter < 10; counter++)
    {
        int rnd = rand.Next(15);
        if (rnd == 0)  buff[counter] = '0';
        if (rnd == 1)  buff[counter] = '1';
        if (rnd == 2)  buff[counter] = '2';
        if (rnd == 3)  buff[counter] = '3';
        if (rnd == 4)  buff[counter] = '4';
        if (rnd == 5)  buff[counter] = '5';
        if (rnd == 6)  buff[counter] = '6';
        if (rnd == 7)  buff[counter] = '7';
        if (rnd == 8)  buff[counter] = '8';
        if (rnd == 9)  buff[counter] = '9';
        if (rnd == 10) buff[counter] = 'A';
        if (rnd == 11) buff[counter] = 'B';
        if (rnd == 12) buff[counter] = 'C';
        if (rnd == 13) buff[counter] = 'D';
        if (rnd == 14) buff[counter] = 'E';
        if (rnd == 15) buff[counter] = 'F';
    }

    GlobalR.Add(chatid, new string(buff));
}

void DCon(long chatId) //disconnect user
{
    GlobalR.Remove(chatId);
}