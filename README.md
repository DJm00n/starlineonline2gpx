# starlineonline2gpx
Программа для конвертирования json/ajax треков с сайта http://starline-online.ru в формат GPX.

# Как вытянуть этот самый json трек в браузере Google Chrome
1. Заходим на https://starline-online.ru/, вводим свой логин и пароль.
[Imgur](https://i.imgur.com/6qWOUxF.jpg)
2. Жмем кнопку клавиатуры F12 - откроется панель DevTools
[Imgur](https://i.imgur.com/bNyxu3w.jpg)
3. Переходим в панель "Network".
[Imgur](https://i.imgur.com/nav7mFS.jpg)
4. В поле "Filter" пишем "route".
[Imgur](https://i.imgur.com/9XQ9qLb.jpg)
5. На сайте выбираем вкладку "Перемещения" на панели слева, выбираем требуемый период и жмем "Показать отчет за период". При этом на панели DevTools появится тврока ответа сервера.
6. Копируем текст ответа "{"meta":{"mileage"" и т.д. в буфер обмена - правой кнопкой по тексту и выбираем "Выделить все", потом снова правой и "Копировать".
[Imgur](https://i.imgur.com/a0bPkle.jpg)
7. Запускаем Блокнот (жмем меню Пуск и пишем "блокнот") и вставляем текст туда. Сохраняем файл. Имя можно любое, хоть "Мой маршрут.txt".
[Imgur](https://i.imgur.com/ytbz1EO.jpg)

# Как отконвертировать этот самый json трек в GPX
1. Скачиваем скомпилированную программу тут https://github.com/DJm00n/starlineonline2gpx/releases. Файл Release.zip
2. Распаковываем.
3. Перетаскиваем файл "Мой маршрут.txt" на starlineonline2gpx.exe.
[Imgur](https://i.imgur.com/4kNN7qq.jpg)
4. Будет создан файл "Мой маршрут.gpx". Все.

# Если что-то пошло не так
Можете созать тикет https://github.com/DJm00n/starlineonline2gpx/issues - постараюсь помочь.


