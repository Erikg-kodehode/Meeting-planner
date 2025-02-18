# 📅 Meeting Planner CLI

**A simple and interactive CLI-based meeting planner using C# and SQLite.**  
Plan, view, edit, and delete meetings with ease.

---

## 📌 Features

✅ **Add Meetings** – Schedule new meetings with title, time, location, and participants.  
✅ **View All Meetings** – List all planned meetings.  
✅ **View Meeting Details** – See full details of a specific meeting.  
✅ **Edit Meetings** – Change title, time, location, description, and participants.  
✅ **Delete Meetings** – Remove meetings when no longer needed.  
✅ **Flexible Time Input** – Supports `HH:mm`, `1400`, `blank` (now+5min), and `null` for no end time.  
✅ **Participant Management** – Easily add or remove attendees.  
✅ **SQLite Database Support** – Persist meetings automatically.

---

## 🚀 Installation & Usage

### **1️⃣ Install .NET**

Ensure you have **.NET SDK installed**.  
Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download).

### **2️⃣ Clone the Repository**

```sh
git clone https://github.com/your-username/meeting-planner.git
cd meeting-planner
```

### **3️⃣ Restore & Build**

```sh
dotnet restore
dotnet build
```

### **4️⃣ Run the Application**

```sh
dotnet run
```

---

## 📂 Database Structure

This project uses **SQLite** for storing meetings. The database schema:

| Column Name    | Type        | Description                               |
| -------------- | ----------- | ----------------------------------------- |
| `Id`           | `INTEGER`   | Unique meeting ID (Auto-increment).       |
| `Title`        | `TEXT`      | Meeting title.                            |
| `Location`     | `TEXT`      | Where the meeting takes place.            |
| `StartTime`    | `DATETIME`  | When the meeting starts.                  |
| `EndTime`      | `DATETIME?` | When the meeting ends (`null` if no end). |
| `CreatedBy`    | `TEXT`      | The creator of the meeting.               |
| `Description`  | `TEXT`      | Brief description of the meeting.         |
| `Participants` | `TEXT`      | Serialized list of participants.          |

---

## 🛠 How to Use Each Feature

### 📌 1. Add a New Meeting

- Run the program and select `1`.
- Enter the **title**, **location**, **start time**, **end time**, and **creator**.
- Add participants (press **Enter** to finish).

### 📜 2. View All Meetings

- Select `2` to display a list of all meetings.

### 🔍 3. View Meeting Details

- Select `3` and enter a **meeting ID**.

### ✏️ 4. Edit a Meeting

- Select `5` and choose a meeting by **ID**.
- Edit:
  - **1** - Title
  - **2** - Location
  - **3** - Start Time (leave blank for now +5 min)
  - **4** - End Time (leave blank for "no end time")
  - **5** - Description
  - **6** - Manage Participants
  - **7** - Save and exit

### ❌ 5. Delete a Meeting

- Select `4` and enter a **meeting ID**.

---

## 🛠 Troubleshooting

### 🔹 Fixing SQLite Errors

If you encounter a **locked database error**, close any programs using the database and try:

```sh
dotnet clean
dotnet build
dotnet run
```

### 🔹 .NET Not Found?

Make sure .NET is installed:

```sh
dotnet --version
```

If missing, download from [here](https://dotnet.microsoft.com/en-us/download).

### 🔹 Reset Database

If your database is broken or missing, delete `MeetingPlanner.db` and restart.

---

## 📜 License

This project is **MIT Licensed**. Feel free to modify and use it!

---

## 🤝 Contributing

Want to contribute?

1. Fork the repository.
2. Create a new branch: `git checkout -b my-feature`
3. Commit changes: `git commit -m "Added new feature"`
4. Push and submit a **Pull Request**.

---
