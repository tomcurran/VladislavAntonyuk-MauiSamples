# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

A collection of 35+ standalone .NET MAUI sample applications by Vladislav Antonyuk. Each top-level folder is a self-contained MAUI app demonstrating a specific feature or pattern, with an associated blog article at vladislavantonyuk.github.io.

## Build Commands

**Prerequisites:** .NET 10 SDK (prerelease) and MAUI workload (`dotnet workload install maui`).

```bash
# Restore and build entire solution
dotnet restore --configfile NuGet.config
dotnet build

# Build a specific sample
dotnet build MauiMaps/MauiMaps.csproj

# Build for a specific platform
dotnet build -f net10.0-android
dotnet build -f net10.0-ios
dotnet build -f net10.0-maccatalyst

# Run UI tests (per-platform, e.g. Android)
dotnet test MauiTests/Client.Android.UITests/Client.Android.UITests.csproj

# Validate all projects build individually
./validate-build.sh   # or validate-build.ps1 on Windows
```

## Build System

- **Solutions:** `MauiSamples.slnx` (all projects), plus occasional feature-specific `.slnx` files
- **SDK:** .NET 10.0 prerelease pinned in `global.json` with `rollForward: latestMinor`
- **Central Package Management:** All NuGet versions in `Directory.Packages.props`; `.csproj` files reference packages without versions
- **Shared build config:** `Directory.Build.props` sets `TargetFrameworks` (android, ios, maccatalyst; windows on Windows only), `UseMaui=true`, `Nullable=enable`, `ImplicitUsings=enable`, C# preview language features
- **Local packages:** `LocalPackages/` contains pre-release `.nupkg` files (e.g., CommunityToolkit.Maui previews); `NuGet.config` adds this as a source
- **Compiled XAML bindings:** `MauiEnableXamlCBindingWithSourceCompilation=true` — XAML must use `x:DataType` for compiled bindings

## Architecture Patterns

**MVVM with source generators:** ViewModels use CommunityToolkit.Mvvm — `[ObservableProperty]` for properties, `[RelayCommand]` for commands, `ObservableObject` as base class. No manual `INotifyPropertyChanged`.

**Dependency injection:** `MauiProgram.cs` in each sample registers services, ViewModels, and Views with the DI container. Common pattern: extension methods like `RegisterAppServices()`, `RegisterViewModels()`, `RegisterViews()`.

**Shell navigation:** Most samples use `AppShell.xaml` for route-based navigation with `AddSingletonWithShellRoute<TView, TViewModel>()`.

**Custom handlers:** Platform-specific behavior via MAUI handlers configured in `MauiProgram.cs` with `ConfigureMauiHandlers()`.

**Per-sample structure:**
- `MauiProgram.cs` — app builder and DI setup
- `App.xaml(.cs)` / `AppShell.xaml(.cs)` — app entry and navigation
- `Views/`, `ViewModels/`, `Models/` — standard MVVM folders
- `Platforms/{Android,iOS,MacCatalyst,Windows}/` — platform-specific code

## Special Directories

- `AndroidBindableLibraries/` — MAUI Android binding libraries wrapping native Java/Kotlin libraries (own `Directory.Build.props` targeting `net10.0-android` only)
- `iOSExtensions/` — iOS App Extensions (Share Extension, Widgets) with own `Directory.Build.props` targeting iOS, `TreatWarningsAsErrors=true`
- `MauiTests/` — Appium-based UI tests; shared test code in `Client.Shared.UITests/` linked into per-platform projects via MSBuild `<Compile Include>`
- `md/` — README generation templates; README.md is generated from `README.mdpp` using Python MarkdownPP

## Key Dependencies

- `Microsoft.Maui.Controls` 10.0.0-rc.2 — core framework
- `CommunityToolkit.Mvvm` 8.4.0 — MVVM source generators
- `CommunityToolkit.Maui` (local pre-release) — extended controls, popups, animations
- `CommunityToolkit.Maui.Maps` — map controls
- `Refit` — type-safe REST clients
- `Microsoft.EntityFrameworkCore.Sqlite` / `sqlite-net-pcl` — local data storage
- `xunit.v3` + `Appium.WebDriver` — UI testing
