# NullAI
=======
NullAI
======
## עברית

מאגר זה מכיל יישום WPF בשם NullAI.
תיעוד נוסף יתווסף בעתיד.

### איך להוריד
- שכפלו את המאגר:
  ```
  git clone https://github.com/your-username/NullAi.git
  ```
- לחלופין, הורידו את קובץ ה-ZIP מדף המאגר וחלצו אותו.

### איך להשתמש
1. התקינו את ערכת הפיתוח של .NET עם עומס העבודה "Windows Desktop".
2. שחזרו ובנו את הפרויקט (הוסיפו `-p:EnableWindowsTargeting=true` בעת בנייה בלינוקס):
   ```
   dotnet restore -p:EnableWindowsTargeting=true
   dotnet build -p:EnableWindowsTargeting=true
   ```
3. הריצו את היישום:
   ```
   dotnet run
   ```
4. ספקו כל מפתח API נדרש בחלון ההגדרות.
   התלויות של OpenCV למערכת ההפעלה משוחזרות אוטומטית עבור Windows 11 ו-Ubuntu 24.04.2 LTS (קרנל 6.12.13) באמצעות חבילות ייחודיות למערכת במהלך שלב זה.

### איך זה עובד
החלון הראשי מאחסן מספר תכונות:
- **צ'אט בינה מלאכותית** – שלחו בקשות טקסט או קול המעובדות על ידי `AIService`.
- **קנבס ציור** – ציירו ישירות על הקנבס באמצעות `DrawingService`.
- **מחולל מודלים תלת־ממדיים** – תארו אובייקט ו-`Model3DService` מייצא מודלים בפורמט `.obj` או `.stl`.
- **פקודות קוליות** – `SpeechService` מקשיב לפקודות ומפעיל תגובות מותאמות או מאומנות דרך `VoiceTrainingService`.
- **זיהוי נוכחות** – זיהוי באמצעות מצלמת רשת (אופציונלי) תוך שימוש ב-OpenCV.
- **בקרות פרטיות** – `PrivacyService` שומר העדפות משתמש לשימוש במיקרופון ומצלמת רשת.

השירותים הללו מתואמים על ידי `MainWindow` ומשתמשים במחלקות עזר כמו `ApiKeyManager` ו-`CustomVoiceCommand`.

### מפתחות API
מפתחות API נשמרים באמצעות `ApiKeyManager` ונשמרים בספריית נתוני היישום של המשתמש דרך `SecureStorage`. ראו `API_KEYS.md` לפרטים. סקריפט עזר `Scripts/test_api_key.py` ניתן לשימוש כדי לוודא שמשתנה הסביבה `OPENAI_API_KEY` מוגדר ונגיש:


 main

## English

This repository contains a WPF application called NullAI.
More documentation will be added in the future.

### How to Download
- Clone the repository:
  ```
  git clone https://github.com/your-username/NullAi.git
  ```
- Alternatively, download the ZIP from the repository page and extract it.

### How to Use
1. Install the .NET SDK with the Windows Desktop workload.
2. Restore and build the project (add `-p:EnableWindowsTargeting=true` when building on Linux):
   ```
   dotnet restore -p:EnableWindowsTargeting=true
   dotnet build -p:EnableWindowsTargeting=true
   ```
3. Run the application:
   ```
   dotnet run
   ```
4. Provide any required API keys in the settings window.
   OpenCV runtime dependencies are restored automatically for Windows 11 and Ubuntu 24.04.2 LTS (kernel 6.12.13) using OS-specific packages during this step.

### How It Works
The main window hosts several features:
- **AI Chat** – send text or voice prompts that are processed by `AIService`.
- **Drawing Canvas** – sketch directly on the canvas through `DrawingService`.
- **3D Model Generator** – describe an object and `Model3DService` exports models in `.obj` or `.stl` format.
- **Voice Commands** – `SpeechService` listens for commands and triggers custom or trained responses via `VoiceTrainingService`.
- **Presence Detection** – optional webcam-based detection using OpenCV.
- **Privacy Controls** – `PrivacyService` stores user preferences for microphone and webcam usage.

These services are coordinated by `MainWindow` and use helper classes such as `ApiKeyManager` and `CustomVoiceCommand`.

### API Keys

API keys are stored using `ApiKeyManager` and persisted to the user's application data directory via `SecureStorage`. See [`API_KEYS.md`](API_KEYS.md) for details. A helper script [`Scripts/test_api_key.py`](Scripts/test_api_key.py) can be used to verify that the `OPENAI_API_KEY` environment variable is set and reachable:

```
python3 Scripts/test_api_key.py
```

## עברית

<div dir="rtl" lang="he">

מאגר זה מכיל יישום WPF בשם NullAI.
תיעוד נוסף יתווסף בעתיד.

### איך להוריד
- שכפלו את המאגר:
  ```
  git clone https://github.com/your-username/NullAi.git
  ```
- לחלופין, הורידו את קובץ ה-ZIP מדף המאגר וחלצו אותו.

### איך להשתמש
1. התקינו את ערכת הפיתוח של .NET עם עומס העבודה "Windows Desktop".
2. שחזרו ובנו את הפרויקט (הוסיפו `-p:EnableWindowsTargeting=true` בעת בנייה בלינוקס):
   ```
   dotnet restore -p:EnableWindowsTargeting=true
   dotnet build -p:EnableWindowsTargeting=true
   ```
3. הריצו את היישום:
   ```
   dotnet run
   ```
4. ספקו כל מפתח API נדרש בחלון ההגדרות.
   התלויות של OpenCV משוחזרות אוטומטית עבור Windows 11 ו-Ubuntu 24.04.2 LTS (קרנל 6.12.13) באמצעות חבילות ייחודיות למערכת במהלך שלב זה.

### איך זה עובד
החלון הראשי מאחסן מספר תכונות:
- **צ'אט בינה מלאכותית** – שלחו בקשות טקסט או קול המעובדות על ידי `AIService`.
- **קנבס ציור** – ציירו ישירות על הקנבס באמצעות `DrawingService`.
- **מחולל מודלים תלת־ממדיים** – תארו אובייקט ו-`Model3DService` מייצא מודלים בפורמט `.obj` או `.stl`.
- **פקודות קוליות** – `SpeechService` מקשיב לפקודות ומפעיל תגובות מותאמות או מאומנות דרך `VoiceTrainingService`.
- **זיהוי נוכחות** – זיהוי באמצעות מצלמת רשת (אופציונלי) תוך שימוש ב-OpenCV.
- **בקרות פרטיות** – `PrivacyService` שומר העדפות משתמש לשימוש במיקרופון ומצלמת רשת.

השירותים הללו מתואמים על ידי `MainWindow` ומשתמשים במחלקות עזר כמו `ApiKeyManager` ו-`CustomVoiceCommand`.

### מפתחות API
מפתחות API נשמרים באמצעות `ApiKeyManager` ונשמרים בספריית נתוני היישום של המשתמש דרך `SecureStorage`. ראו [`API_KEYS.md`](API_KEYS.md) לפרטים. סקריפט עזר [`Scripts/test_api_key.py`](Scripts/test_api_key.py) ניתן לשימוש כדי לוודא שמשתנה הסביבה `OPENAI_API_KEY` מוגדר ונגיש:
=======
API keys are stored using `ApiKeyManager` and persisted to the user's application data directory via `SecureStorage`. See `API_KEYS.md` for details. A helper script `Scripts/test_api_key.py` can be used to verify that the `OPENAI_API_KEY` environment variable is set and reachable:
 main

```
python3 Scripts/test_api_key.py
```


</div>
=======

python3 Scripts/test_api_key.py
```
```
 main

