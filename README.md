# Project Plan

#### Azure SQL Server 資料庫設計

- BikeStatus

  - 單車索引 BikeStatusID
  - PurchaseBikeID (來自 PurchaseBike 的 PurchaseBikeID)
  - 租借狀態 RentalStatus ( 0 表示未出借 , 1 表示已出借)
  - 車況 BikeStatus( null 表示正常, 0 表示送修中, 1 表示報廢)

```
Example：
1 PurchaseBikeID 0 0
2 PurchaseBikeID 0 0
```

- PurchaseBike

  - 採購索引 PurchaseBikeID
  - 品名 BikeName
  - 車種 BikeModel
  - 廠商 Manufacturer
  - 採購數量 Quantity
  - 採購價格 (單件) Price
  - 採購日期 Date
  - 狀態 PurchaseStatus (0 正常, 1 作廢)

```
Example：
1 M12 鋁合金環島公路車 公路車 美利達 2 10000 2022/11/18 0
2 M22 鋁合金街區淑女車 淑女車 美利達 4 10000 2022/11/18 0
```

- BikeAccount
  - 索引
  - 帳號 Account
  - 密碼 Password 需 salt
  - 名稱 Name

```
Example：
1 admin  pwd  管理者
2 userA  pwd  使用者A

```

- BikeModel

  - 索引 BikeModelID
  - 車種 Model
    - 電動車
    - 公路車
    - 登山車
    - 淑女車

- BikeManufacturer
  - 索引 BikeManufacturerID
  - 廠商 Manufacturer
    - 美利達
    - 捷安特
    - 卜赫馬
    - 道卡斯

---

#### 後端

- 技術框架
  - Asp .NET Core Web API
  - OpenAPI
  - JWT
  - Dapper
  - Git
  - Azure
- API

  - CRUD
    - PurchaseBike
    - BikeStatus
    - BikeAccount
    - BikeModel
    - BikeManufacturer

- TSQL
- 使用者密碼做完雜湊才匯入資料庫

---

#### 前端

- Login Page
  - 輸入帳號、密碼
  - 註冊機制 (待補)
    - 輸入帳號、密碼 (判斷有沒有這個帳號，可以補核准機制，畢竟是後台)
- One Tab
  - 單車庫存狀態
    - 同一類型的單車 已租借件數/未租借件數
    - 租借時長、租借價格
- Two Tab
  - 採購資訊
    - 匯入採購單後，會依照採購數量去產生對應的單車資料，並賦於每個單車唯一編號
- Three Tab
  - OpenAPI 連結
- 登出 Button

---
