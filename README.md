Database:
/Databases/Staff.db

# Тема
Менеджер персоналу. Програма для керування розкладом користувачів. Планування подій (графік роботи, дата зарплати, заходи) у вигляді календарю

# Мета
Десктопний додаток, розроблений за допомогою технології WPF, націлений на користування в локальній мережі.

# Загальний функціонал
+ Диференціація користувачів програми на ролі (Admin, Manager, User)
+ Система індивідуальних графіків подій / звіту роботи 

## Ролі користувачів

### User / Manager
+ Перегляд графіку подій (дата\час роботи, дні зарплати)
+ Звіт роботи (відмітка присутності)

### Manager
+ Встановлення графіку для User

### Admin
+ Створення користувачів (генерація логіну та паролю, симуляція комерційного видання акаунтів). Користувачі надалі можуть самостійно доповнювати профілі інформацією

## Елементи керування
Елементами керування є GUI, що включає в себе систему з меню різних вікон (всі вікна існують тільки в одному екземплярі та неодночасно). 
Перелік вікон:
+ Вікно входу / реєстрації
+ Вікно меню
+ MessageBox для підтвердження дій

### Графік подій
Є одним з секцій меню. В себе включає календар, на якому в зручному вигляді показано "розклад" роботи та список 

### Вікно входу
+ Поле вводу логіну (Login)
+ Поле вводу паролю (Password)
+ Кнопка входу (Login) - переходить на вікно меню
+ Кнопка реєстрації для самостійного заповнення (Sign Up) - змінює поля вводу на:
    + Ім'я (Full Name) - в форматі Ім'я Прізвище
    + Логін (Login)
    + Кнопка Згенерувати (Generate) з правої сторони від вводу логіну - генерація логіну за введеним ім'ям (Формат: Surn_aa11, де Surn - перші 4 літери прізвища транслітеровані англійською, aa - 2 випадкові букви, 11 - 2 випадкові цифри)
    + Пароль (Password)
    + Кнопка Згенерувати (Generate) з правої сторони від вводу паролю - генерація випадкового паролю
    + Підтеврдження паролю (у випадку генерації також заповнюється автоматично)
    + Телефон (Phone)
    + Адреса електронної пошти (Email)
    + Кнопка реєстрації (Sign Up) - після реєстрації потрібно увійти в аккаунт
    + Повернення до входу (Back to Log In)
### Основне вікно
+ Розділи меню:
 + Розклад (Schedule)
    + Кнопки розкладу на день/тиждень/місяць (day/week/month)
 + Персонал (Personnel) - перегляд співробітників та можливості для адміністраторів :
    + Додати подію (Add Event) 
    + Редагування інформації про співробітника (Edit Staff)
      + Можливість змінити телефон, пошту, пароль та роль
 + Розділ додавання персоналу (Add user) - тільки для адміністраторів з реєстрацією нового користувача по даним: 
    + Ім'я (Full Name) - в форматі Ім'я Прізвище
    + Логін (Login)
    + Кнопка Згенерувати (Generate) з правої сторони від вводу логіну - генерація логіну за введеним ім'ям (Формат: Surn_aa11, де Surn - перші 4 літери прізвища транслітеровані англійською, aa - 2 випадкові букви, 11 - 2 випадкові цифри)
    + Пароль (Password)
    + Кнопка Згенерувати (Generate) з правої сторони від вводу паролю - генерація випадкового паролю
    + Підтеврдження паролю (у випадку генерації також заповнюється автоматично)
    + Телефон (Phone)
    + Адреса електронної пошти (Email)
    + Кнопка реєстрації (Sign Up) - після реєстрації потрібно увійти в аккаунт
    + Повернення до входу (Back to Log In)
 + Інформація про себе (My profile) - перегляд та зміна деякої інформації:
    + Зміна телефону (кнопка Change з правоъ сторони від поля Phone)
    + Зміна пошти (кнопка Change з правоъ сторони від поля Email)
    + Кнопка зміни паролю (Change Password) відкриває вікно:
        + Поле вводу старого паролю (Old Password)
        + Поле вводу нового паролю (New Password)
        + Кнопка підтвердження зміни (Change Password)
+ Кнопка виходу з аккаунту(Log Out)