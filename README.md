# Track Management API

نظام لإدارة الفنانين (Artists)، الأغاني (Tracks)، وتوزيعها على منصات البث الرقمي (DSPs) مثل Spotify و Apple Music و YouTube Music.

المشروع مبني بـ **.NET 10 Web API** (Clean Architecture) في الباك إند، و **Angular** في الفرونت إند.

---

## المتطلبات (Prerequisites)

قبل ما تشغّل المشروع، لازم يكون عندك مثبّت:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (نسخة 18 أو أحدث) و npm
- [Angular CLI](https://angular.dev/tools/cli) — تثبيتها: `npm install -g @angular/cli`
- SQL Server (يفضّل SQL Server Express أو LocalDB)
- (اختياري) [Postman](https://www.postman.com/) لتجربة الـ endpoints يدوياً

---

## هيكل المشروع (Project Structure)

```
TrackManagement/
├── TrackManagement.API/            # طبقة الـ API (Controllers, Program.cs)
├── TrackManagement.Application/    # DTOs, Interfaces, Validators
├── TrackManagement.Domain/         # Entities, Enums
├── TrackManagement.Infrastructure/ # DbContext, Migrations, Services
└── TrackManagement.UI/             # Angular Frontend
```

---

## 1. تشغيل الـ Backend (.NET API)

### أ) اضبط الـ Connection String

افتح `TrackManagement.API/appsettings.json` وتأكد إن الـ `ConnectionStrings:DefaultConnection` بيشاور على السيرفر بتاعك:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TrackManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

> غيّر `YOUR_SERVER_NAME` باسم السيرفر المحلي بتاعك (تلاقيه في SSMS، أو جرّب `.\SQLEXPRESS` أو `(localdb)\MSSQLLocalDB`).

### ب) شغّل الـ Migrations

من داخل مجلد `TrackManagement.API`:

```bash
dotnet ef database update --project ..\TrackManagement.Infrastructure
```

ده هيعمل إنشاء لقاعدة البيانات `TrackManagementDb` تلقائياً ويطبّق كل الجداول، بالإضافة إلى الـ Seed Data التالية:

- 3 فنانين (Artists)
- 8 أغاني (Tracks) بأنواع وحالات مختلفة
- 3 منصات (DSPs): Spotify, Apple Music, YouTube Music

> لو عايزة تعمل migration جديدة بعد أي تعديل على الـ Entities:
>
> ```bash
> dotnet ef migrations add <MigrationName> --project ..\TrackManagement.Infrastructure
> dotnet ef database update --project ..\TrackManagement.Infrastructure
> ```

### ج) شغّل الـ API

```bash
dotnet run
```

هيشتغل على: `http://localhost:5110` (تأكد من الرقم الفعلي في رسالة `Now listening on: ...` اللي بتظهر في الـ console، وعدّليه في الفرونت إند لو مختلف).

يمكنك فتح `http://localhost:5110/openapi/v1.json` أو استخدام Swagger/OpenAPI (في بيئة Development) لاستعراض كل الـ endpoints.

---

## 2. تشغيل الـ Frontend (Angular)

من داخل مجلد `TrackManagement.UI`:

```bash
npm install
ng serve
```

هيشتغل على: `http://localhost:4200`

> تأكد إن الـ `apiUrl` في كل من `src/app/services/track.service.ts` و `src/app/services/auth.service.ts` بيطابق الـ port الفعلي بتاع الـ backend.

---

## 3. كيفية الحصول على JWT Token

### من الواجهة (الطريقة الموصى بها)

1. افتح `http://localhost:4200/login`
2. سجّل دخول بالبيانات التالية:
   - **Username:** `admin`
   - **Password:** `password123`
3. بعد نجاح الدخول، التوكن بيتخزن تلقائياً ويتضاف لكل الطلبات المحمية (POST/PATCH) عن طريق HTTP Interceptor .
4. زرار **تسجيل خروج** بيمسح التوكن ويرجّعك لصفحة الدخول.

### من Postman / يدوياً

```http
POST http://localhost:5110/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

الـ Response هيرجّع:

```json
{ "token": "eyJhbGciOi..." }
```

استخدم التوكن ده في أي endpoint محمي عن طريق الـ Header:

```
Authorization: Bearer <token>
```
