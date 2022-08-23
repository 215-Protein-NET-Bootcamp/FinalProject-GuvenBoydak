## Patika  Protein .Net Bootcamp'inin bitirme projesi olarak yaptığım uygulama.
## *Kullanıcıların giriş işlemleri yaptıkları ve Ürünlere teklif vererek veya direk satın aldıkları mini e Ticaret uygulaması.*
#### *Projede kullanılan teknoloji ve kütüphaneler*
- Dapper 
- JWT Authentication
- Hangfire BackgroundJop
- RabbitMQ
- PostgreSql
- Fluent Migrations
- Serilog
- HMACSHA512 şifreleme algoritması
- Autofact 
- Fluent Validation
- AutoMapper
- XUnit Test with Mock
- Generic Repository pattern

### Postman Api dökümantastonu için [Tıklayınız](https://documenter.getpostman.com/view/15763755/VUqpuHvg) 


### Üye İşlemleri
- Kullanıcı ilgili bilgileri doldurup kayıt olabiliyor.Ardından E mail ile bir Guid activasyon code gönderiliyor ve kişinin Activasyon işlemlerini tamamlanıyor.
- Kullanıcı Email ve Pasword ile sisteme giriş yapabiliyor. Ardından Kullanıcıya Hoş geldiniz maili gönderiliyor.
- Kullanıcı girdigi şifre kontrol ediliyor 3 kez yanlış girilen şifrelerde Hesap Bloke ediliyor. Ardındna kullanıcının mail adresine Guid ile code gönderilip bloke işlemini kaldırılmasını saglıyoruz.
- Kullanıcı istedigi zaman şifresini degiştirip yeni bir şifre alabiliyor.
- Kulanıcıların aktif, pasif veya veri tabanındaki tüm kayıtlarını listeliyoruz.
### Ürün işlemleri
- Kullanıcılar satıcakları ürünleri Kategori, Renk, Marka, Kullanım durumu gibi özelliklerle ekleyebiliyor.
- Kulanıcılar bir ürüne sadece 1 resim yükleyebliyor ve resim boyutu maksimum 400kb olabiliyor. 
- Kullanıcının ürünlerini veri tabanından çekip listeliyebilir.
- Ürünleri aktif, pasif veya tüm ürünleri listeliyoruz.
- Ürünleri belirtilen sayı kadar sayfa sayfa listeliyoruz.
### Teklif işlemleri
- Kullanıcı teklif vericegi ürünün isOfferable alanı kontrol edilip teklif verebilmesini saglıyoruz.
- Kullanıcının yaptıgı teklifleri listeliyoruz.
- Belirtilen Ürüne yapılan teklifleri listeliyoruz.
- Kullanıcının ürünlerine gelen teklifleri listeliyoruz.
- Kullanıcı istedigi ürüne teklif vermeden direk satın alabiliyor. Satın aldıktan sonra bu ürüne yapılan teklifleri siliyoruz.
- Kullanıcı gelen teklife göre Onaylama işlemi gerçekleştiriyor. Ardındad teklifi onaylanan ürünün bilgilerini günceleyip bu ürüne yapılan teklifleri siliyoruz.
### Kategori işlemleri
- Belirtilen kategoriye göre ürünleri listeliyoruz.
- Kategorileri aktif, pasif veya tüm ürünleri listeliyoruz.
 <hr>
 
# Database Diagram
![Diagram](https://i.hizliresim.com/iw387vc.png)
 
 <hr>
 
 ## 
 ## Projenin Kurulumu
 - Projeyi aşagıdaki adresden biligisayarınıza klonlayabilirsiniz
 ````
 https://github.com/215-Protein-NET-Bootcamp/FinalProject-GuvenBoydak.git
 ````
 - Proje’yi çalıştırmak için PostgreSql'in bilgisayarımızda yüklü ve çalışıyor olması gerekmektedir. Daha Sonra FinalProject.Api katmanındakı ``appsettings.json`` dosyası içerisindeki baglantı adreslerini sırasıyla kendi database ayaralarınıza göre değiştirmelisiniz. 
 - PostgreSql'de HangFire Database ve proje Database'i  çalıştırıyoruz. Fluent migrations ile postgresql e tabloları oluşturabilmesi için bir database yaratmanız gerekiyor.
 - Projeyi çalıştırmak için Solution dosyası üzerinden property diyip daha sonra Multiple Start-Up projesi seçilerek FinalProject.API ve FinalProject.RabbitMqConsumer aynı anda işaretlenmesiniz. Daha sonra Swagger veya postman üzerinde api'yi kullanabilirsiniz.
 
 ````
 ConnectionStrings": {
  "PostgreSql": "User ID=postgres; Password=(Şifre); Server=localhost; Port=5432 ;Database=FinalProject; Integrated Security=true; Pooling=true"
}, 

"RabbitMq": "ilgili rabbitmq amps instance bilgisi",

"HangfirePostreSql": "User ID=postgres; Password=(Şifre); Server=localhost; Port=5432 ;Database=FinalProject; Integrated Security=true; Pooling=true",
  ````

 
