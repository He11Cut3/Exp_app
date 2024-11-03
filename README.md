# Expenses App

<a href="https://ibb.co/mqTtF8n"><img src="https://i.ibb.co/YkQ8TZ5/2024-11-03-142333.png" alt="2024-11-03-142333" border="0"></a>

![Static Badge](https://img.shields.io/badge/Framework-ASP.NET_8.0-purple?logo=dotnet) ![Static Badge](https://img.shields.io/badge/Language-C%23-purple?logo=csharp) ![Static Badge](https://img.shields.io/badge/DataBase-MSSQL-purple?logo=microsoftsqlserver)


Простое веб-приложение на ASP.NET Core для ведения учета расходов с использованием MS SQL Server. Приложение позволяет добавлять, редактировать, удалять и просматривать расходы и категории расходов, а также формировать отчеты.

## Функционал

- **CRUD для расходов**: добавление, редактирование, удаление и просмотр расходов с указанием категории, суммы, даты и комментария.
- **CRUD для категорий**: добавление, редактирование, удаление и просмотр категорий расходов.
- **Группировка по месяцам**: расходы отображаются в виде графика, сгруппированного по месяцам.
- **Экспорт в Excel**: возможность выгрузки отчета в Excel с отдельными листами для каждого месяца.
  
## Установка

### Шаг 1: Клонирование репозитория

Клонируйте репозиторий на свой компьютер с помощью команды:

```bash
git clone https://github.com/username/expenses-app.git
cd expenses-app
```
### Шаг 2: Смена строки подключения

Необходимо в файле ```appsettings.json``` изменить строку подключения:
```bash
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
### Шаг 3: Установка и подключения базы данных

Варианты:
* Установить через скрипт. Скрипт ```Exp_Data.sql```
* Восстановить через BackUp. BackUp ```Expenses_db.bak```
* С помощью миграции.
  * Запуск команды ```dotnet ef migrations add InitialCreate```
  * Применение миграции ```dotnet ef database update ```

## Разработчик

- [He11Cut3](https://github.com/He11Cut3)
