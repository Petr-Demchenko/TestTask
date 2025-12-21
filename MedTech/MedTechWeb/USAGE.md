# Как использовать страницу ExportPage.aspx

## Быстрый старт

1. **Откройте страницу в браузере**
   ```
   http://localhost:55000/Pages/ExportPage.aspx
   ```

2. **Нажмите кнопку "Экспорт"**
   - Кнопка станет неактивной
   - Появится спинер загрузки
   - На сервере будут получены данные из БД
   - Будет создан Excel файл

3. **Скачайте файл**
   - После завершения спинер исчезнет
   - Файл автоматически загрузится в ваш браузер
   - Кнопка "Экспорт" станет доступной снова

## Настройка базы данных

### 1. Обновите Web.config

Откройте файл `Web.config` и добавьте строку подключения к вашей базе данных:

```xml
<configuration>
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=YOUR_SERVER; Database=YOUR_DATABASE; Trusted_Connection=true;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

### 2. Обновите запрос в DataManager.cs

Откройте файл `Managers/DataManager.cs` и замените запрос:

```csharp
var query = @"SELECT TOP 100 * FROM YourTableName";
```

На ваш собственный SQL запрос.

## Пример: Экспорт данных о пациентах

### Шаг 1: Обновите запрос в DataManager.cs

```csharp
var query = @"
    SELECT 
        PatientID,
        FirstName,
        LastName,
        BirthDate,
        Email,
        PhoneNumber
    FROM Patients
    WHERE IsActive = 1
    ORDER BY LastName, FirstName";
```

### Шаг 2: Запустите страницу

1. Откройте `http://localhost:55000/Pages/ExportPage.aspx`
2. Нажмите кнопку "Экспорт"
3. Дождитесь загрузки
4. Скачайте Excel файл с данными пациентов

## Пример: Экспорт статистики

### Шаг 1: Обновите запрос в DataManager.cs

```csharp
var query = @"
    SELECT 
        MONTH(VisitDate) AS Month,
        COUNT(*) AS TotalVisits,
        COUNT(DISTINCT PatientID) AS UniquePatients,
        AVG(CAST(DurationMinutes AS FLOAT)) AS AvgDuration
    FROM Visits
    WHERE YEAR(VisitDate) = YEAR(GETDATE())
    GROUP BY MONTH(VisitDate)
    ORDER BY Month";
```

### Шаг 2: Экспортируйте статистику

1. Откройте `http://localhost:55000/Pages/ExportPage.aspx`
2. Нажмите кнопку "Экспорт"
3. Excel файл будет содержать месячную статистику

## Обработка ошибок

Если что-то пошло не так:

### 1. Ошибка подключения к БД
- Проверьте строку подключения в `Web.config`
- Убедитесь, что сервер БД доступен
- Проверьте права доступа учетной записи

### 2. Ошибка "Нет данных для экспорта"
- Проверьте SQL запрос в `DataManager.cs`
- Убедитесь, что в таблице есть данные

### 3. Ошибка при создании Excel файла
- Проверьте логи Windows Event Log
- Убедитесь, что у вас достаточно памяти для большого файла
- Попробуйте ограничить количество строк в запросе

## Производительность

### Для больших объемов данных

Если вы экспортируете больше 100,000 строк:

1. **Увеличьте timeout в DataManager.cs**
   ```csharp
   command.CommandTimeout = 600; // 10 минут
   ```

2. **Использутйте пагинацию в SQL**
   ```csharp
   var query = @"SELECT * FROM YourTable WHERE ID BETWEEN @StartID AND @EndID";
   ```

3. **Оптимизируйте запрос**
   - Используйте индексы
   - Ограничьте количество колонок в SELECT
   - Добавьте WHERE условия для фильтрации

## Безопасность

### CSRF защита
- Страница проверяет referrer для защиты от CSRF атак
- Убедитесь, что вы открываете страницу с правильного домена

### XSS защита
- Все выводимые данные HTML-кодируются
- Имена файлов проверяются на валидность

### SQL Injection защита
- Используйте параметризованные запросы при добавлении фильтров
- Не конкатенируйте пользовательский ввод в SQL

## Логирование

Все ошибки логируются в Windows Event Log в категорию "Application" с Source "MedTechWeb".

Для просмотра логов:
1. Откройте Event Viewer
2. Перейдите в Application
3. Найдите записи с Source "MedTechWeb"

## CSS и JavaScript

### Кастомизация спинера

Отредактируйте CSS в `Pages/ExportPage.aspx`:

```css
.spinner {
    border: 4px solid #f3f3f3;
    border-top: 4px solid #007bff;  /* Измените цвет здесь */
    border-radius: 50%;
    width: 50px;                      /* Измените размер здесь */
    height: 50px;
    animation: spin 1s linear infinite;
    margin: 0 auto 15px;
}
```

### Кастомизация цветов

Обновите переменные цветов в CSS:

```css
/* Синий цвет для спинера и заголовков */
border-top: 4px solid #007bff;

/* Темно-серый цвет для текста */
color: #333;

/* Светло-серый цвет для фона */
background-color: #f5f5f5;
```

## Развертывание на Production

1. **Создайте отдельный Web.config для Production**
   ```xml
   <compilation debug="false" targetFramework="4.8" />
   ```

2. **Установите HTTPS**
   - Купите SSL сертификат
   - Обновите IIS конфигурацию

3. **Оптимизируйте производительность**
   - Включите кэширование
   - Сжимайте HTTP ответы
   - Используйте CDN для статических файлов

4. **Мониторьте логи**
   - Регулярно проверяйте Windows Event Log
   - Используйте Application Insights или подобный сервис

## Поддержка

Если у вас возникли проблемы:

1. Проверьте логи в Windows Event Log
2. Убедитесь, что все зависимости установлены (ClosedXML, DocumentFormat.OpenXml)
3. Проверьте, что проект правильно развернут
4. Свяжитесь с командой поддержки MedTech
