# Lexicon LMS – grupp 4

> **Tillfällig README.** Den uppdateras när LMS-templaten läggs in i repot.

Slutprojekt på Lexicon: en lärplattform (LMS) byggd med Blazor Web App, remote API, YARP/BFF och EF Core.

## Länkar

| Vad | Länk |
|---|---|
| Projektboard | https://github.com/orgs/Lexicon-grupp4/projects/1 |
| Azure (LMS.API) | https://lms2-api.azurewebsites.net/ |
| Deploy-workflow | [`.github/workflows/main_lms-grupp4.yml`](.github/workflows/main_lms-grupp4.yml) |

# Data Model
> Utkast. Uppdateras när entiteterna är bestämda och skapade i `Domain.Models`.

```mermaid
erDiagram
 
    Course ||--o{ Module : "has"

    Module ||--o{ Activity : "has"

    ActivityType ||--o{ Activity : "type of"
 
    Course ||--o{ ApplicationUser : "has users"
 
    Course ||--o{ Document : "has"

    Module ||--o{ Document : "has"

    Activity ||--o{ Document : "has"
 
    ApplicationUser ||--o{ Document : "uploads"

    ApplicationUser ||--o{ Notification : "receives"

    ApplicationUser ||--o{ Submission : "submits"

    ApplicationUser ||--o{ Feedback : "gives"
 
    Activity ||--o{ Submission : "has"

    Submission ||--o{ Feedback : "has"
 
    Course {

        int Id PK

        string Name

        string Code

        string Description

        datetime StartDate

        datetime EndDate

    }
 
    Module {

        int Id PK

        int CourseId FK

        string Name

        string Description

        datetime StartDate

        datetime EndDate

    }
 
    ActivityType {

        int Id PK

        string Name

    }
 
    Activity {

        int Id PK

        int ModuleId FK

        int ActivityTypeId FK

        string Title

        string Description

        datetime StartTime

        datetime EndTime

    }
 
    ApplicationUser {

        string Id PK

        string FirstName

        string LastName

        string Email

        int CourseId FK

    }
 
    Document {

        int Id PK

        int ActivityId FK 

        int CourseId FK 

        int ModuleId FK 

        string FileName

        string FilePath

        datetime UploadedAt

        string UploadedById FK

    }
 
    Submission {

        int Id PK

        int ActivityId FK

        string UserId FK

        string FileName

        string FilePath

        datetime SubmittedAt

        string Status

    }
 
    Feedback {

        int Id PK

        int SubmissionId FK

        string UserId FK

        string Comment

        string Grade

        datetime CreatedAt

    }
 
    Notification {

        int Id PK

        string UserId FK

        string Message

        bool IsRead

        datetime CreatedAt

    }
 ```

## Branches

| Branch | Syfte |
|---|---|
| `main` | Verifierade leveranser. Push hit deployar till Azure. Inga direkta commits. |
| `development` | Default-branch. Alla feature-PRs går hit. |
| `feature/usXX-kort-namn` | Ny funktionalitet, t.ex. `feature/us04-modules` |
| `bugfix/kort-namn` | Buggrättningar, t.ex. `bugfix/submission-deadline` |

## Arbetsflöde

1. Välj en sub-issue på boarden, tilldela dig själv och flytta den till **In progress**.
2. Skapa en branch från `development`, gärna direkt från issuet:
   ```bash
   gh issue develop <nr> --name feature/us04-modules --base development --checkout
   ```
3. Implementera och verifiera lokalt.
4. Öppna en PR mot `development`. Skriv `Closes #nr` för varje sub-issue som PR:n färdigställer.
5. En annan gruppmedlem granskar och godkänner.
6. Merge. Sub-issues stängs och flyttas till **Done** automatiskt.
7. `development` → `main` via PR när gruppen har en verifierad leveranspunkt.

Håll högst 2–3 PRs samtidigt i Ready for review. Prioritera review när kön växer.

## Statusar på boarden

| Status | Betydelse |
|---|---|
| Backlog | Kravet finns men är inte valt för aktivt arbete. |
| Sprintbacklog | Valt för den aktuella sprinten. |
| In progress | Någon arbetar aktivt med uppgiften. |
| In review | En PR är Ready for review. |
| Done | Granskat, mergat och verifierat. |

## Definition of Done 

- [ ] Uppfyller issuebeskrivningen och berörda acceptance criteria.
- [ ] Bygger utan fel och kan startas.
- [ ] Server-side validering och authorization är på plats.
- [ ] Databasändringar har en fungerande migration.
- [ ] Klientens API-anrop går via YARP/BFF-flödet.
- [ ] Testscenarier kontrollerade, automatiserade tester passerar.
- [ ] PR granskad av minst en annan gruppmedlem och kommentarer hanterade.
- [ ] Mergad till `development` och berörda sub-issues stängda.
- [ ] README uppdaterad vid ändrad installation eller konfiguration.

Sprint Goal 1

Skapa en stabil grund för LMS:et genom att verifiera templaten och GitHub-flödet samt bygga den grundläggande strukturen för användare och kurser.

## Kom igång

_Fylls i när templaten finns: krav (.NET 10 SDK), secrets, databas och hur API och Blazor startas._

## Deploy

Push till `main` kör GitHub Actions som bygger och publicerar `LMS.API` till Azure App Service `lms2-api`. Azure-inloggningen gäller bara körningar från `main`.
