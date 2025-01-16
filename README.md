<h1 align="center">
  <br>
  <a href="https://77.222.38.141"><img src="http://postimg.su/image/wGGKTsMz/browser_hAlMsSHAk0.png" alt="Alfa Telekom" width="300"></a>
  <br>
</h1>
<h4 align="center">Веб-приложение для автоматизации и управления процессами
биллинга мобильного оператора.</h4>

> [!IMPORTANT]  
> Дисклеймер: Данный сайт является студенческим учебным проектом, созданным исключительно в образовательных целях. Он не является официальным проектом, не связан с деятельностью и не одобрен АО "АЛЬФА-БАНК". Вся представленная информация используется только для обучения, и любой контент, относящийся к АО "АЛЬФА-БАНК", использован в рамках добросовестного использования без умысла на коммерческую выгоду или нарушение прав.

## Описание
Cистема предназначена для автоматизации всех процессов биллинга мобильного оператора с обеспечением следующей функциональности: управление счетами абонентов, создание счетов,
расчеты по тарифным планам, учёт списаний за услуги и пополнений баланса;

## MVP
Биллинговая система оператора сотовой связи.

* Минимальные функции сервиса:
  - Ведение базы клиентов
  - Ведение базы услуг
  - Ведение базы тарифов
  - Произведение расчёта с абонентом
  - Возможность принимать оплаты

## Разработано с помощью
<div align="left">
  <h3>Backend:</h3>
  <a href="https://dotnet.microsoft.com/en-us/"><img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dot-net/dot-net-plain-wordmark.svg" height="60" alt="dot-net logo"  /></a>
  <img width="12" />
  <a href="https://dotnet.microsoft.com/en-us/apps/aspnet"><img src="https://user-images.githubusercontent.com/54532837/236268072-6a9e6dd3-9e9f-41ee-b4ba-c4df8aa11886.png" height="60" alt="aspnet logo"  /></a>
  <img width="12" />
  <a href="https://www.postgresql.org/"><img src="https://raw.githubusercontent.com/marwin1991/profile-technology-icons/refs/heads/main/icons/postgresql.png" height="60" alt="psql logo"  /></a>
  <img width="12" />
  <a href="https://learn.microsoft.com/ru-ru/aspnet/entity-framework"> <img src="https://github.com/campusMVP/dotnetCoreLogoPack/blob/master/Entity%20Framework%20Core/Bitmap%20RGB/Bitmap-MEDIUM_Entity-Framework-Logo_2colors_Square_RGB.png?raw=true" height="60" alt="ef logo"  /></a>
  <img width="12" />
  <a href="https://www.quartz-scheduler.org/"><img src="https://dz2cdn1.dzone.com/storage/temp/14824345-1622849995838.png" height="60" alt="quartz logo"  /></a>
  <img width="12" />
  </br>
  <h3>Frontend:</h3>
  <a href="https://www.w3.org/html/"><img src="https://github.com/tandpfun/skill-icons/blob/main/icons/HTML.svg" height="60" alt="html logo"  /></a>
  <img width="12" />
  <a href="https://www.w3.org/Style/CSS/Overview.en.html"><img src="https://github.com/tandpfun/skill-icons/blob/main/icons/CSS.svg" height="60" alt="css logo"  /></a>
  <img width="12" />
  <a href="https://react.dev/"><img src="https://github.com/tandpfun/skill-icons/blob/main/icons/React-Dark.svg" height="60" alt="react logo"  /></a>
  <img width="12" />
</div>

## Команда
| Тимлид-аналитик | Backend-Разработчик | Backend-Разработчик | Frontend-Разработчик | Дизайнер |
| :---: | :---: | :---: | :---: | :---: |
| [Погирейчик Андрей](https://t.me/Hackathon_lover) | [Беликов Никита](https://t.me/holo21k) | [Верхотуров Виталий](https://t.me/ArcKontyR) | [Егоров Евгений](https://t.me/callme_jewel) | [Уварова Ольга](https://t.me/ol_rey) |

## Ссылки

[Репозиторий](https://github.com/twentythirtyone/BillingApplication) &nbsp;&middot;&nbsp;
[Диск](https://disk.yandex.ru/d/wwAHT5EUN9RB5g) &nbsp;&middot;&nbsp;
[Ссылка на сайт](https://77.222.38.141)

## Запуск:
> рабоатет только из git bash (если не знаешь как то переименуй [example.env](example.env) в .env)

### Создать .env файл
```sh
make env
```
## Локальный запуск

- Создать бд
```sh
make db
```

Если проблема с скриптами:
```sh
dos2unix docker/scripts/entrypoint.sh
dos2unix docker/scripts/wait-for-it.sh
```

- Строка подключения к бд
```
  "db_connection": "Host=localhost;Port=6432;Database=billingapp;Username=billing;Password=password;Pooling=true"
```

- Запустить приложение через http 
```sh
cd BillingApplication.Server
dotnet run --launch-profile "http"
```

### [http://localhost:5173](http://localhost:5173)

## Запуск через docker

- Строка подключения к бд
```
  "db_connection": "Host=pgbouncer;Port=6432;Database=billingapp;Username=billing;Password=password;Pooling=true"
```

- Фоновый запуск
```sh
make run
```

- Или запуск с логами
```sh
make runl
```

### [https://localhost](https://localhost)

## Удалить контейнеры (в том числе и с бд)
```sh
make off
```
