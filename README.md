# MeterReading - React Drag & Drop File Upload

This project demonstrates a React app integrated with an API that allows users to upload CSV files containing meter readings. The app includes a drag-and-drop interface for uploading CSV files, with feedback on the number of successful and failed uploads.

## Project Structure

- **API**: The backend API is built with ASP.NET Core and handles CSV file uploads and database operations.
- **ReactApp**: The frontend is a React application that allows users to drag and drop CSV files for upload.

## Prerequisites

Before running this project, make sure you have the following installed:

- [Node.js](https://nodejs.org/) (for React app)
- [npm](https://www.npmjs.com/) (for React app)
- [ASP.NET Core SDK](https://dotnet.microsoft.com/download) (for API)

## Frontend (React)

The frontend is a React app that allows users to upload CSV files. The app uses `axios` to send the file to the API.

### Setup

1. Navigate to the `frontend` directory:

   ```bash
   cd frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Run the development server:

   ```bash
   npm start
   ```

   This will run the React app on [http://localhost:3000](http://localhost:3000).

### Commands

- **Run React App**: `npm start` — Starts the React development server on [http://localhost:3000](http://localhost:3000).
- **Build React App**: `npm run build` — Builds the production-ready version of the app.
  
## Backend (API)

The backend is built with ASP.NET Core and handles CSV file uploads and processing.

### Setup

1. Open the solution in Visual Studio or VS Code.
2. Restore the required NuGet packages.

   ```bash
   dotnet restore
   ```

3. Run the API:

   ```bash
   dotnet run
   ```

   The API will run on [https://localhost:7047](https://localhost:7047) by default.

### API Endpoints

- **POST /meter-reading-uploads**: Upload a CSV file containing meter readings.
  - **Request**: A `multipart/form-data` request with the CSV file attached.
  - **Response**: A JSON object indicating how many records were successfully uploaded and how many failed.

## How the Upload Works

1. The user drags and drops a CSV file into the React app's interface.
2. The frontend sends the file to the backend via a POST request.
3. The backend processes the file, inserts valid records into the database, and returns the count of successful and failed uploads.

## Technologies Used

- **React**: Frontend user interface with drag-and-drop file upload.
- **ASP.NET Core**: Backend API to process the file and handle database operations.
- **CSV File Parsing**: The backend parses the CSV file to extract meter readings and validates them before insertion into the database.

## Notes

- Ensure CORS is properly configured in the API for the React app to communicate with it.
- The backend uses a SQL Server database to store meter readings.

## Troubleshooting

- **CORS Error**: Make sure the API has CORS headers configured to allow requests from your frontend (localhost:3000).
- **API Not Running**: Check if the API is running on the correct port (`https://localhost:7047`) and verify there are no issues with the database connection.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Certainly! Here's the updated section with the tables listed before the stored procedures, as you would execute them first:

---

### Database Scripts Set Up

Before running the application, ensure you have the following database scripts executed in your SQL Server database.

#### 1. **Creating User**

To allow specific access for the `MeterReadingAdmin` user:

```sql
/****** Object:  User [MeterReadingAdmin]    Script Date: 10/03/2025 22:09:26 ******/
CREATE USER [MeterReadingAdmin] FOR LOGIN [MeterReadingAdmin] WITH DEFAULT_SCHEMA=[dbo]
GO
```

#### 2. **Tables**

Ensure the following tables are created in your database:

- **Accounts Table**: This table stores account details.
  
```sql
CREATE TABLE [dbo].[Accounts](
 [AccountId] [bigint] NOT NULL,
 [FirstName] [nvarchar](50) NULL,
 [LastName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
 [AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
```

- **MeterReadings Table**: This table stores the meter readings.
  
```sql
CREATE TABLE [dbo].[MeterReadings](
 [AccountId] [bigint] NOT NULL,
 [MeterReadingDateTime] [datetime] NOT NULL,
 [MeterReadValue] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
 [AccountId] ASC,
 [MeterReadingDateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MeterReadings]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([AccountId])
ON DELETE CASCADE
GO
```

#### 3. **User-Defined Type**

The `MeterReadingTableType` is a user-defined table type that will be used for bulk inserts:

```sql
CREATE TYPE [dbo].[MeterReadingTableType] AS TABLE(
 [AccountId] [bigint] NULL,
 [MeterReadingDateTime] [datetime] NULL,
 [MeterReadValue] [nvarchar](10) NULL
)
GO
```

#### 4. **Stored Procedures**

- **GetAllAccounts**: Retrieves all accounts.

```sql
CREATE PROCEDURE [dbo].[GetAllAccounts]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Accounts;
END;

GO
```

- **InsertMeterReadings**: This procedure will insert new meter readings. It uses a `MERGE` statement to insert data only if it does not already exist in the database.

```sql
CREATE PROCEDURE [dbo].[InsertMeterReadings]
    @MeterReadings MeterReadingTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO [dbo].[MeterReadings] AS target
    USING @MeterReadings AS source
    ON target.AccountId = source.AccountId AND target.MeterReadingDateTime = source.MeterReadingDateTime
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (AccountId, MeterReadingDateTime, MeterReadValue)
        VALUES (source.AccountId, source.MeterReadingDateTime, source.MeterReadValue);

    -- Return the number of rows inserted
    SELECT @@ROWCOUNT AS InsertedCount;
END;
GO
```
