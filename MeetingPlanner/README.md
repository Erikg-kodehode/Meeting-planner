# 🗓️ Meeting Planner - CLI-Based Scheduler

A simple **CLI-based meeting planner** for managing meetings, including **creating, editing, viewing, and deleting meetings**. This planner supports **dynamic time input**, including minutes from now, full time formats, and flexible participant management.

## 📥 Installation

1. **Clone this repository**:

   ```sh
   git clone https://github.com/YOUR-USERNAME/MeetingPlanner.git
   cd MeetingPlanner
   ```

2. **Ensure you have .NET installed**:

   ```sh
   dotnet --version
   ```

   If not installed, download from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet).

3. **Build the project**:

   ```sh
   dotnet build
   ```

4. **Run the meeting planner**:
   ```sh
   dotnet run
   ```

---

## 📌 Features

✅ **Create new meetings** with title, location, time, description, and participants.  
✅ **View all scheduled meetings** in a list format.  
✅ **See full details of a specific meeting** (title, time, location, description, participants).  
✅ **Edit an existing meeting** (change title, time, location, description, or participants).  
✅ **Delete a meeting** permanently.  
✅ **Smart time input system**:

- **"14:00"** → Stays as **14:00**
- **"1400"** → Converts to **14:00**
- **"45"** → Adds **45 minutes from now**
- **Blank input** → Defaults to **now +5 minutes** (start) or **no end time** (end)  
  ✅ **Manage meeting participants** (add or remove attendees).

---

## 🛠️ How to Use

Once you run the program (`dotnet run`), you'll see the following menu:

```
===== MØTEPLANLEGGER =====
1. Legg til et møte
2. Vis alle møter
3. Se detaljer for et møte
4. Slett et møte
5. Rediger et møte
6. Avslutt
Velg et alternativ:
```

### **🆕 Adding a Meeting**

1. Select **option 1** ("Legg til et møte").
2. Enter meeting details:
   - **Tittel** (meeting title)
   - **Sted** (location)
   - **Starttid** (input format below ⬇️)
   - **Slutttid** (input format below ⬇️)
   - **Beskrivelse** (short description)
   - **Deltakere** (list participants, press Enter to stop)
3. Meeting is saved! ✅

### **🔍 Viewing Meetings**

- **Option 2**: Shows all scheduled meetings.
- **Option 3**: Enter meeting ID to see full details.

### **✏️ Editing a Meeting**

1. Select **option 5** ("Rediger et møte").
2. Choose a meeting by ID.
3. Select what to edit (Title, Location, Time, Description, Participants).
4. Changes are saved instantly. ✅

### **🗑️ Deleting a Meeting**

- **Option 4**: Select a meeting ID, confirm deletion.

---

## ⏰ Time Input Formats

When setting **start** or **end time**, you can use these formats:

| Input Format   | Behavior                                                |
| -------------- | ------------------------------------------------------- |
| `14:00`        | **Exact time (14:00)**                                  |
| `1400`         | **Converts to 14:00**                                   |
| `45`           | **Adds 45 minutes from now**                            |
| `1`-`3` digits | **Minutes from now**                                    |
| _(blank)_      | **Defaults to now +5 min (start) or no end time (end)** |

---

## 🛠️ Troubleshooting

### **Meeting changes are not saved**

- Make sure you **press enter after each edit**.
- If using SQLite, ensure no **other processes are locking the DB**.

### **Errors when running the program**

- Try rebuilding the project:
  ```sh
  dotnet clean
  dotnet build
  ```
- If the database is corrupted, **delete and recreate it**:
  ```sh
  rm meetingplanner.db
  dotnet run
  ```

---

## 🤝 Contributing

1. **Fork this repository**
2. **Create a new branch** (`git checkout -b feature-new`)
3. **Commit your changes** (`git commit -m "Added new feature"`)
4. **Push to the branch** (`git push origin feature-new`)
5. **Create a Pull Request**

---

## 📜 License

This project is licensed under the **MIT License**.

---
