﻿Программа отслеживает телеграм-сообщения указанных пользователей и перенаправляет их в указанный чат
Для работы программы надо:
1) Создать своё приложение согл https://core.telegram.org/api/obtaining_api_id
1.1) В конфигурационный файл записать параметры приложения (куда что писать указано в самом конфиг. файле) 
1.2) В конфиг. файл записать свой телефон. Это нужно только для первого запуска - при запуске на телефон(в телеграм) придёт код подтверждения, который надо ввести
2) Запустить приложение. Кнопочками GetContactList/GetChatList вывести списки своих контактов и чатов, определить идентифиикаторы пользователей , которых надо отслеживать (и в каких чатах)
2.1) В конфиг. файл записать пользователей и чаты, которые отслеживаются
3) Создать бота в BotFather (гуглите, это прямо в телеграмме делается)
3.1) В конфигурационный файл записать параметры бота
4) Создать чат, куда перенаправлять отслеживаемые сообщения. Добавить в чат созданного бота.
4.1) В конфигурационный файл записать параметры чата
5) Запустить приложение, запустить мониторинг - "RunMonitor"