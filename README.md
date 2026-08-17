# Track Management API

نظام لإدارة الفنانين (Artists)، الأغاني (Tracks)، وتوزيعها على منصات البث الرقمي (DSPs) مثل Spotify و Apple Music و YouTube Music.

المشروع مبني بـ **.NET 10 Web API** (Clean Architecture) في الباك إند، و **Angular** في الفرونت إند.

---

## المتطلبات (Prerequisites)

قبل ما تشغّلي المشروع، لازم يكون عندك مثبّت:

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

### أ) اضبطي الـ Connection String

افتحي `TrackManagement.API/appsettings.json` وتأكدي إن الـ `ConnectionStrings:DefaultConnection` بيشاور على السيرفر بتاعك:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TrackManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

> غيّري `YOUR_SERVER_NAME` باسم السيرفر المحلي بتاعك (تلاقيه في SSMS، أو جرّبي `.\SQLEXPRESS` أو `(localdb)\MSSQLLocalDB`).

### ب) شغّلي الـ Migrations

من داخل مجلد `TrackManagement.API`:

```bash
dotnet ef database update --project ..\TrackManagement.Infrastructure
```

ده هيعمل إنشاء لقاعدة البيانات `TrackManagementDb` تلقائياً ويطبّق كل الجداول، بالإضافة إلى الـ Seed Data التالية:

- 3 فنانين (Artists)
- 8 أغاني (Tracks) بأنواع وحالات مختلفة
- 3 منصات (DSPs): Spotify, Apple Music, YouTube Music

> لو عايزة تعملي migration جديدة بعد أي تعديل على الـ Entities:
>
> ```bash
> dotnet ef migrations add <MigrationName> --project ..\TrackManagement.Infrastructure
> dotnet ef database update --project ..\TrackManagement.Infrastructure
> ```

### ج) شغّلي الـ API

```bash
dotnet run
```

هيشتغل على: `http://localhost:5110` (تأكدي من الرقم الفعلي في رسالة `Now listening on: ...` اللي بتظهر في الـ console، وعدّليه في الفرونت إند لو مختلف).

يمكنك فتح `http://localhost:5110/openapi/v1.json` أو استخدام Swagger/OpenAPI (في بيئة Development) لاستعراض كل الـ endpoints.

---

## 2. تشغيل الـ Frontend (Angular)

من داخل مجلد `TrackManagement.UI`:

```bash
npm install
ng serve
```

هيشتغل على: `http://localhost:4200`

> تأكدي إن الـ `apiUrl` في كل من `src/app/services/track.service.ts` و `src/app/services/auth.service.ts` بيطابق الـ port الفعلي بتاع الـ backend.

---

## 3. كيفية الحصول على JWT Token

### من الواجهة (الطريقة الموصى بها)

1. افتحي `http://localhost:4200/login`
2. سجّلي دخول بالبيانات التالية:
   - **Username:** `admin`
   - **Password:** `password123`
3. بعد نجاح الدخول، التوكن بيتخزن تلقائياً ويتضاف لكل الطلبات المحمية (POST/PATCH) عن طريق HTTP Interceptor — مفيش حاجة تانية مطلوبة منك.
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

استخدمي التوكن ده في أي endpoint محمي عن طريق الـ Header:

```
Authorization: Bearer <token>
```

---

## 4. الـ Endpoints المتاحة

| Method | Route                                  | محمي بـ JWT؟ | الوصف                          |
| ------ | -------------------------------------- | ------------ | ------------------------------ |
| POST   | `/api/auth/login`                      | لا           | تسجيل الدخول والحصول على توكن  |
| GET    | `/api/artists`                         | لا           | عرض كل الفنانين                |
| POST   | `/api/artists`                         | نعم          | إضافة فنان جديد                |
| GET    | `/api/tracks?artistId=&genre=&status=` | لا           | عرض الأغاني مع فلترة اختيارية  |
| GET    | `/api/tracks/{id}`                     | لا           | تفاصيل أغنية وحالات توزيعها    |
| POST   | `/api/tracks`                          | نعم          | إضافة أغنية جديدة              |
| PATCH  | `/api/tracks/{id}/status`              | نعم          | تحديث حالة الأغنية             |
| POST   | `/api/tracks/{id}/distribute`          | نعم          | توزيع الأغنية على منصة أو أكثر |
| GET    | `/api/dsps`                            | لا           | عرض منصات التوزيع المتاحة      |

---

## 5. ملاحظات أمنية

راجعي ملف **[DECISIONS.md](./DECISIONS.md)** للاطلاع على القرارات التقنية والأمنية المتخذة أثناء بناء المشروع، بما فيها استخدام أدوات الـ AI.

---

## استكشاف الأخطاء الشائعة (Troubleshooting)

| المشكلة                                          | الحل                                                                          |
| ------------------------------------------------ | ----------------------------------------------------------------------------- |
| `ERR_CONNECTION_REFUSED` عند تسجيل الدخول        | تأكدي إن الـ backend شغال فعلاً (`dotnet run`) وإن الـ port في `apiUrl` مطابق |
| خطأ `Duplicate 'Migration' attribute` عند البناء | امسحي مجلد `Migrations` بالكامل وأعيدي إنشاء الـ migration من جديد            |
| 401 Unauthorized عند الإضافة/التعديل             | تأكدي إنك مسجلة دخول (التوكن منتهي الصلاحية بعد ساعتين)                       |
