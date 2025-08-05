# Vamino_WebAPI# 💼 Vaminu Backend - ASP.NET 9 Web API (Layered Architecture)

پروژه‌ی Web API بک‌اند «وامینو» با معماری لایه‌ای پیاده‌سازی شده و به زبان #C و فریم‌ورک .NET 9 توسعه یافته است. این API عملیات‌هایی مانند مدیریت کاربران، ثبت درخواست وام، ارزیابی اعتبار و اتصال به درگاه پرداخت را انجام می‌دهد.

---

## 🏗️ معماری لایه‌ای پروژه

```
Vaminu.WebApi/         ← لایه API (نمای بیرونی - Controllers)
Vaminu.Application/    ← لایه منطق تجاری (Business Logic)
Vaminu.Infrastructure/ ← لایه دسترسی به داده و سرویس‌ها (DbContext, External Services)
Vaminu.Domain/         ← لایه مدل‌های دامنه (Entities, Enums, Interfaces)
```

**✅ مزایا:** جداسازی وظایف، تست‌پذیری، نگهداری آسان، رعایت اصل SOLID

---

## ⚙️ تکنولوژی‌های استفاده‌شده

- .NET 9 Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- JWT Authentication
- Swagger (Swashbuckle)
- Serilog Logging

---

## 🚀 اجرای پروژه

1. اطمینان از نصب .NET 9 SDK
2. اجرای Migrations و ساخت دیتابیس:

```bash
dotnet ef database update --project Vaminu.Infrastructure
```

3. اجرای پروژه:

```bash
dotnet run --project Vaminu.WebApi
```

---

## 📁 مسیرها و توضیح لایه‌ها

| لایه | توضیحات |
|------|---------|
| `Vaminu.WebApi` | نقطه ورودی اصلی API، شامل Controllerها و Middleware |
| `Vaminu.Application` | شامل Serviceها، Validatorها و Business Rules |
| `Vaminu.Infrastructure` | شامل EF Core DbContext، Repositoryها، و اتصال به درگاه‌ها |
| `Vaminu.Domain` | مدل‌های دامنه، اینترفیس‌ها و Enumها |

---

## 🔑 احراز هویت JWT

تمامی APIهای حساس، با توکن JWT محافظت شده‌اند.

```
Authorization: Bearer {token}
```

---

## 📚 مستندسازی Swagger

پس از اجرای پروژه:

```
https://localhost:5001/swagger/index.html
```

---

## 🧪 تست

(در صورت اضافه‌کردن پروژه تست)

```bash
dotnet test
```

---

## ✍️ مشارکت

- رعایت قوانین Clean Code
- کامیت‌های واضح و تمیز
- تست‌ قبل از Push

---

## 📜 مجوز

این پروژه توسط تیم توسعه وامینو ایجاد شده و کلیه حقوق برای شرکت توسعه فناوران آینده هوشمند محفوظ است.
