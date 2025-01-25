# LibraryApi
API untuk manajemen peminjaman buku di perpustakaan.
- Tambah buku dengan inputan : judul buku, penerbit, penulis, tahun terbit dan gambar sampul.
- Order buku dengan inputan nama peminjam dan durasi hari peminjaman.
- Pencarian berdasarkan judul yang dapat menampilkan informasi detail buku beserta status peminjaman.
- Pencatatan log untuk setiap request dan response API.

## Teknologi yang Digunakan

- **Backend**: ASP.NET Core 8
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **API**: RESTful API
- **Logging**: Middleware custom untuk logging request dan response

## Setup Project
#### konfigurasi database SQL server lokal
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=LibraryApi;User Id=sa;Password=Pass123!;TrustServerCertificate=True;"
}
```


## Endpoints
### 1. Contoh Request dan Response untuk create buku baru
#### Request: `POST /api/Books`
```json
{
  "title": "Sejarah Filsafat Barat",
  "author": "Bertrand Russell",
  "publisher": "Pustaka Pelajar",
  "publishedYear": 1946,
  "coverImageUrl": "https://d28hgpri8am2if.cloudfront.net/book_images/cvr9780671201586_9780671201586_hr.jpg"
}
```
#### Response: `201 Created`
```json
{
  "id": 22,
  "title": "Sejarah Filsafat Barat",
  "author": "Bertrand Russell",
  "publisher": "Pustaka Pelajar",
  "publishedYear": 1946,
  "coverImageUrl": "https://d28hgpri8am2if.cloudfront.net/book_images/cvr9780671201586_9780671201586_hr.jpg",
  "isAvailable": true,
  "willBeAvailableAt": "Tersedia saat ini"
}
```
### 2. Contoh Request dan Respons untuk order buku
#### Request: `POST /api/Orders`
```json
{
  "bookId": 22,
  "borrowerName": "Budi si Peminjam Buku",
  "borrowDuration": 7
}
```
#### Response: `201 Created`
```json
{
  "id": 13,
  "borrowerName": "Budi si Peminjam Buku",
  "startBorrowDate": "25 January 2025",
  "endBorrowDate": "01 February 2025",
  "borrowDuration": 7,
  "bookId": 22,
  "bookTitle": "Sejarah Filsafat Barat"
}
```
### 3. Pencarian berdasarkan judul
#### request: `api/Books/search?title=filsafat`
#### response: `200 Ok`
```json
  {
    "id": 22,
    "title": "Sejarah Filsafat Barat",
    "author": "Bertrand Russell",
    "publisher": "Pustaka Pelajar",
    "publishedYear": 1946,
    "coverImageUrl": "https://d28hgpri8am2if.cloudfront.net/book_images/cvr9780671201586_9780671201586_hr.jpg",
    "isAvailable": false,
    "willBeAvailableAt": "01 February 2025"
  }
```


