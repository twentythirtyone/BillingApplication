# ПРИДУМАЙТЕ НОРМ РИДМИ ПЖ

## А пока просто распишу запуск (для локали только секрет чуть другой)
> рабоатет только из git bash (если не знаешь как то переименуй [example.env](example.env) в .env)

### создать .env файл
```sh
make env
```
## Локальный запуск

- Создать бд
```sh
make db
```

если какой то трабл со скриптами
```sh
dos2unix docker/scripts/entrypoint.sh
dos2unix docker/scripts/wait-for-it.sh
```

- Строка подключения к бд
```
  "db_connection": "Host=localhost;Port=6432;Database=billingapp;Username=billing;Password=password;Pooling=true"
```

- Запустить приложение через http (одному деду морозу ведомо работает ли через https)
> С наступающим 🎄🎅
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

## удалить контейнеры (в том числе и с бд)
```sh
make off
```