# 📘 Automation Framework & Full-Stack Web Application 

This repository includes two main parts:  

1. **Web Application** – Backend (Node.js/Express) + Frontend (React).  
2. **Automation Framework** – Selenium + NUnit test suite to validate the app.  

---

## ⚙️ 1. Running the Web Application  

### ✅ Prerequisites
- Install **Node.js (>= 16.x)** → [Download here](https://nodejs.org/en/download/)  
- Install **npm** (comes with Node)  
- Install **Git** → [Download here](https://git-scm.com/)  

---

### ▶️ Steps

#### 1. Clone the repository
```bash
git clone https://github.com/alexhippo/full-stack-app-with-react-rest-api.git
cd full-stack-app-with-react-rest-api

#### 3. Start the Frontend

```bash
cd api
npm install
npm run seed   # initializes the database with sample data
npm start
```

Backend available at:

```
http://localhost:5000
```

#### 3. Start the Frontend

Open a new terminal:

```bash
cd client
npm install
npm start
```

Frontend available at:

```
http://localhost:3000
```

⚠️ **Start the backend before the frontend.**

---

## 🧪 2. Running the Automation Framework

The automation framework is written in **C#**, using **NUnit** and **Selenium WebDriver**.

### ✅ Prerequisites

* Install **.NET 8 SDK** → [Download here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Install **Visual Studio 2022** (or later) with:

  * `.NET desktop development` workload
  * `ASP.NET and web development` workload
* Install **Git**
* Browsers supported: **Chrome**, **Edge**, **Firefox** (drivers managed by Selenium Manager)

---

### ▶️ Steps

#### 1. Clone the automation framework repository

```bash
git clone https://github.com/Jenson-08/DotnetAutomation.git
cd DotnetAutomation
```

#### 2. Configure Test Settings

Edit `TestSettings.json`:

```json
{
  "BaseUrl": "http://localhost:3000",
  "Browser": "chrome",     // chrome | edge | firefox
  "Headless": false,
  "ExplicitWaitSec": 10,
  "ImplicitWaitMs": 0
}
```

#### 3. Restore dependencies

```bash
dotnet restore
```

#### 4. Run all tests

```bash
dotnet test
```

#### 5. Run specific tests

Run only `LoginTests`:

```bash
dotnet test --filter "FullyQualifiedName~UI.Tests.Example.LoginTests"
```

Run only one test case:

```bash
dotnet test --filter "Name=Login_WithValidCredentials_ShouldRedirectToDashboard"
```

---

### 📊 Test Reports

After execution, an **Extent Report** (`.html`) will be generated inside:

```
DotnetAutomation\tests\UI.Tests\Reports
```

Open it in your browser to review detailed test results.

---

## 📂 Project Structure (Automation)

```
DotnetAutomation/
│
├── Framework/             # Core framework code
│   ├── Config/            # Test settings
│   ├── Drivers/           # WebDriver factory
│   ├── Pages/             # Page Object Models
│   └── Data_Driven/       # Test data
│   
│
├── tests/
│   └── UI.Tests/          # NUnit test project
│       ├── Hooks/         # Helpers (AuthHelper, etc.)
│       ├── Example/       # LoginTests, CourseTests
│
└── ExtentReports/         # Test execution reports
```

---

## 📋 Notes

* Always start the **backend (port 5000)** and **frontend (port 3000)** before running automation tests.
* Tests run on **Windows, Linux, or macOS** with .NET 8 and supported browsers.


