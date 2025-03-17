@echo off
REM RandomBot.cmd - Run the bot in development or release mode
REM Set MODE=dev for development (default, always rebuilds)
REM Set MODE=release for release (only runs if bin exists)

set MODE=dev

if "%MODE%"=="dev" (
    REM Development mode: always clean, build, and run
    rmdir /s /q bin 2>nul
    rmdir /s /q obj 2>nul
    dotnet build
    dotnet run --no-build
) else if "%MODE%"=="release" (
    REM Release mode: no rebuild if bin exists
    if exist bin\ (
        dotnet run --no-build
    ) else (
        dotnet build
        dotnet run --no-build
    )
) else (
    echo Error: Invalid MODE value. Use "dev" or "release".
)
pause
