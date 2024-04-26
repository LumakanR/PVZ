from aiogram import Bot

API_TOKEN = '7149293032:AAHl2sNwAAv3cOzeAN9Jy7s7W6Ga0uPjftk'
bot = Bot(token=API_TOKEN)

#Заказ прибыл в пункт выдачи
async def send_message(user_id, message):
    await bot.send_message(chat_id=1198440826, text='Ваш заказ был доставлен в пункт выдачи')