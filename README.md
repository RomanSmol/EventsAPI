# Welcome to my repo! 🎉🎉🎉

## Here is quick setup tutorial! 🚀
### STEP 1. Pull the repo
### STEP 2. Compose the docker image of SQL Server (docker compose file here -> /Infrastucture) `docker-compose up -d`
### STEP 3. Apply the EF migrations `dotnet ef database update --project ./Presentation`
### STEP 4. Run app `dotnet run`
### STEP 5. Stay hydrated!


# 📂 Project Structure
/Application    - Core business logic
/Domain         - Entities and core domain models
/Infrastructure - Database and external services
/Presentation   - API
/Tests - Unit tests
