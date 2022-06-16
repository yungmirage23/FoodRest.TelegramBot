# TelegramBot
## Бот развёрнут на Azure / Ник бота @ShopBotKr_bot https://t.me/ShopBotKr_bot


https://user-images.githubusercontent.com/103426361/174075898-6aa0bf8a-01a3-415f-a6e3-345c034aca77.MP4

+ API Который принимает номер телефона, сохраняет в кеш 4-х значный код. Далее Пользователь
```csharp
[HttpPost]
        public async Task<int> Post([FromBody]string phoneNumber)
        {
            var user =dataContext.Users.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
            var code =confirmationServiceservice.CreateConfirmationCode(phoneNumber);
            if (user != null)
            {
                await telegramClient.SendTextMessageAsync(user.Id, "Ваш код подтверждения:", ParseMode.Markdown);
                await telegramClient.SendTextMessageAsync(user.Id, $"{code}", ParseMode.Markdown);
            }
            return code;
        }
```
![Alt Text](https://s8.gifyu.com/images/telegram_confirm.gif)
