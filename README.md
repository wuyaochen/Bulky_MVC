# Bulky Book - E-Commerce Web App (.NET 8)

以 ASP.NET Core `.NET 8` 開發的電商網站，重點展示後端能力：身分驗證/授權、EF Core 資料建模、購物車/訂單流程、Session 狀態管理，以及 Stripe Checkout 金流串接。

> 本專案為地端作品集專案（未部署）。

## Links
- GitHub: https://github.com/wuyaochen/Bulky_MVC
- Demo Video:

---

## Security / Demo Notes（重要）
本專案主要用途為 **面試展示/練習 .NET 工具鏈與架構**，不作為正式上線產品使用。

- **不包含任何有效的 Secrets/金鑰**
  - Repo 內不提交 Stripe `SecretKey`、第三方 OAuth `client secret`、SMTP 密碼等敏感資訊。
  - 第三方登入設定若曾出現過舊憑證，也已重設（舊值已無效）。
- **若需啟用金流/第三方登入**
  - 請以本機環境設定（User Secrets / 環境變數）注入設定值，不要提交到 Git。
- **XSS（跨站腳本）風險說明**
  - 專案中部分頁面為了展示「富文字商品描述」會使用 `Html.Raw(...)` 直接輸出 HTML。
  - 這在正式環境可能造成 Stored XSS。若要上線，建議：
    - 對富文字輸入做 HTML Sanitization（白名單允許的標籤/屬性）
    - 搭配設定 CSP（Content-Security-Policy）等安全標頭，降低注入腳本的影響面
- **Seeded Admin**
  - 本專案啟動時可能建立預設角色與 Admin 帳號（僅供本機/示範用途）。
  - 正式環境不建議將任何帳密輸出到 log，應改為以安全的初始流程（環境變數、一次性邀請、強制改密碼等）處理。

---

## Tech Stack	

- **Backend**: C#, ASP.NET Core (MVC + Areas), Razor Pages (Identity UI)
- **Auth**: ASP.NET Core Identity、Role-based Authorization
- **Data**: Entity Framework Core、SQL Server
- **Architecture**: Repository Pattern + Unit of Work
- **Payment**: Stripe Checkout（可選）
- **State**: Session（`SessionShoppingCart`）
- **Tooling**: Visual Studio、Git/GitHub

---

## Features

### Customer
- 商品列表 / 商品詳細頁
- 加入購物車、調整數量、移除商品（同步更新 Session 購物車數量）
- 結帳摘要頁（填入收件資料、建立訂單）
- 建立訂單資料：`OrderHeader` + `OrderDetail`
- 數量階梯定價（依購買數量自動套用 `Price` / `Price50` / `Price100`）
- Stripe Checkout：建立 Payment Session、付款成功後確認並更新訂單/付款狀態

### Admin
- 訂單管理：更新收件/物流資訊、開始處理、出貨、取消、（已付款時）退款處理
- 使用者管理：
  - 角色管理（Customer / Company / Admin / Employee）
  - 使用者鎖定/解鎖（Lock/Unlock）

### Identity & Initialization
- 啟動時自動執行：
  - 套用尚未執行的 EF Core Migrations
  - 建立預設角色與 Admin 帳號（Seed）

---

## Project Structure

- `BulkyWeb/`：Web 專案
  - `Areas/Customer/`：前台（Customer）Controllers/Views
  - `Areas/Admin/`：後台（Admin）Controllers/Views
  - `Areas/Identity/`：Identity UI（Razor Pages）
- `Bulky.DataAccess/`：`DbContext`、Repositories、Migrations、DB seeding
- `Bulky.Models/`：Entity Models / ViewModels
- `Bulky.Utility/`：共用常數（例如角色/狀態 `SD.cs`）

---

## Local Setup (Windows)

### Prerequisites
- Visual Studio 2026
- .NET SDK 8
- SQL Server（`SQLEXPRESS` 或 LocalDB）

### 1) 設定 Database 連線字串
編輯 `BulkyWeb/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=Bulky;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 2) 建立/更新資料庫 (Migrations)

**方式 A：Package Manager Console**
- Startup Project：`BulkyWeb`
- Default Project：`Bulky.DataAccess`
- 執行：`Update-Database`

**方式 B：EF CLI**
- `dotnet ef database update`

### 3) 執行專案
- 以 Visual Studio 啟動 `BulkyWeb`（F5）

啟動時會自動嘗試：
- 套用 pending migrations
- 建立預設角色 + Admin 帳號（第一輪啟動最明顯）

---

## Default Roles & Seeded Admin

角色定義（`Bulky.Utility/SD.cs`）：
- `Admin`
- `Employee`
- `Customer`
- `Company`

Seeded Admin（`Bulky.DataAccess/DbInitializer/DbInitializer.cs`）：
- Email/UserName：`admin@gmail.com`
- Password：首次初始化時隨機產生，並輸出到應用程式 log（僅供本機開發使用）

> 若要公開 repo，建議改掉 seeded 密碼或移除 seeded admin。 

---

## Stripe (Optional)

本專案已整合 Stripe Checkout。若要啟用，設定 `BulkyWeb/appsettings.json`：

- `Stripe:SecretKey`
- `Stripe:PublishableKey`

> 若不需要金流示範，可不填 `SecretKey`，其餘功能仍可展示（商品/購物車/訂單/後台/權限等）。