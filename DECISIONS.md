## 1. القرارات المعمارية (Architectural Decisions)

### **Clean Architecture في الـ Backend (.NET 10 Web API)**
تم تصميم وبناء الـ Backend بالكامل بـ .NET 10 استناداً إلى معمارية Clean Architecture وتقسيمه إلى 4 طبقات أساسية لضمان فصل المسؤوليات (Separation of Concerns) وسهولة الصيانة والاختبار:
- **Domain:** تحتوي على الكيانات الأساسية (`Artist`, `Track`, `DSP`, `TrackDistribution`) والـ Enums الخاصة بالحالات والتصنيفات بدون أي اعتماديات خارجية.
- **Application:** تحتوي على الـ DTOs، والـ Interfaces للخدمات والـ Repositories، بالإضافة إلى الـ Validation Rules.
- **Infrastructure:** تحتوي على `ApplicationDbContext` وإعدادات Entity Framework Core والـ Migrations والـ Seed Data.
- **API:** تحتوي على الـ Controllers وتكوين الـ Dependency Injection وإعدادات الـ JWT Authentication والـ CORS Policy.

### **Angular 19 في الـ Frontend (مُصمم بمساعدة الـ AI)**
- تم الاستعانة بأدوات الـ AI لتسريع بناء واجهات الفرونت إند وتوليد المكونات بـ **Angular 19**.
- استخدام **Standalone Components** لتقليل التعقيد والتخلص من الـ NgModules التقليدية.
- استخدام **HTTP Interceptors** لإرفاق توكن الـ JWT أوتوماتيكياً مع كل طلب يخرج للـ API المحمية (`POST`, `PATCH`).
- إدارة حالة تسجيل الدخول وتخزين الـ Token في `localStorage` مع التوجيه التلقائي للمستخدم عبر **Angular Router Guards**.

---

## 2. استخدام الذكاء الاصطناعي (AI Assistance)

تم الاقتصار على استخدام أدوات الـ AI في جزئيتين محددتين فقط لزيادة الإنتاجية وتسريع التطوير:
1. **Frontend Development (Angular 19):** المساعدة في إنشاء الهيكل الخارجي للواجهات، ربط الـ Interceptors، وضبط تنسيقات الصفحة.
2. **Documentation (README.md):** توليد وتنسيق ملف الشرح والتعليمات الخاصة بتشغيل المشروع بطريقة منظمّة وواضحة للمراجعين.

---

## 3. الأمن والتحقق (Security & Authentication)

- **JWT Authentication:** تم اعتماد نظام JWT لتوثيق المستخدمين، حيث ينتهي صلاحية التوكن بعد ساعتين لضمان الأمان.
- **Role & Access Control:** الـ Endpoints الخاصة بالقراءة (`GET`) متاحة للجميع، بينما العمليات الحساسة (الإضافة، التحديث، والتوزيع) تتطلب توكن معتمد (`[Authorize]`).
- **CORS Policy:** تم إعداد سياسة CORS في .NET تسمح للـ Frontend (`http://localhost:4200`) بالتواصل السلس مع الـ Backend.

---

## 4. قرارات قاعدة البيانات (Database & EF Core)

- **Fluent API Configuration:** تم تحديد العلاقات بين الكيانات بوضوح (مثل علاقة Many-to-Many بين الـ `Track` والـ `DSP` عبر جدول الوسيط `TrackDistribution`).
- **Seed Data:** تم إضافة بيانات أولية تلقائية (3 فنانين، 8 أغاني بحالات مختلفة، و 3 منصات DSP) لتسهيل تجربة وتشغيل المشروع فوراً بعد الـ Migration.

---

## 5. التحديات والحلول التقنية (Technical Challenges & Insights)

أثناء بناء المشروع وتكامله بين الـ Backend والـ Frontend، واجهنا عدة تحديات تم حلها كالتالي:

1. **إعدادات الـ CORS والتكامل مع الفرونت إند:**
   - *التحدي:* ربط واجهات Angular مع الـ .NET API بدون مشاكل في صلاحيات المتصفح.
   - *الحل:* ضبط سياسة `UseCors` في `Program.cs` بدقة لتسمح بالـ Origin الخاص بـ Angular (`http://localhost:4200`) واستقبال الـ Headers المطلوبة للـ JWT.

2. **التعامل مع ملفات Git المضمنة (Embedded Repositories):**
   - *التحدي:* وجود مجلد `.git` مخفي داخل `TrackManagementUI` تسبب في تنبيه Git واعتباره Submodule غير مكتمل.
   - *الحل:* فك الارتباط من الـ Index وإزالة مجلد `.git` الداخلي لإعادة إدراج مجلد الفرونت إند بالكامل ضمن مستودع المشروع الرئيسي.

3. **التحقق من صحة المدخلات (Data Validation):**
   - *التحدي:* التأكد من عدم توزيع الأغنية على نفس المنصة أكثر من مرة بنفس الحالة.
   - *الحل:* إضافة معالجة منطقية في طبقة الـ Application للـ Backend تضمن التحقق من وجود التوزيع مسبقاً قبل الحفظ في قاعدة البيانات.

---

## 6. الخطوات المستقبلية للتطوير (Future Enhancements)

- إضافة دعم لرفع ملفات الصوت الحقيقية (Audio Uploads) وتخزينها على Cloud Storage (مثل AWS S3).
- إضافة Refresh Tokens لزيادة أمان جلسات المستخدمين.
- كتابة Unit Tests و Integration Tests لطبقتي الـ Application والـ API.
