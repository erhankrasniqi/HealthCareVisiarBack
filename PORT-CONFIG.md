# Port Configuration

This API is configured to run on **fixed ports** to ensure consistency across all environments.

## Default Ports

- **HTTPS:** `7036`
- **HTTP:** `5246`

## Configuration Files

The ports are configured in multiple places to ensure they remain consistent:

### 1. `Program.cs`
```csharp
builder.WebHost.UseUrls("https://localhost:7036", "http://localhost:5246");
```

### 2. `appsettings.json` / `appsettings.Production.json`
```json
{
  "Urls": "https://localhost:7036;http://localhost:5246"
}
```

### 3. `launchSettings.json`
```json
{
  "https": {
    "applicationUrl": "https://localhost:7036;http://localhost:5246"
  }
}
```

## Frontend Configuration

Your frontend application should connect to:
```
https://localhost:7036
```

## Why Fixed Ports?

- ? Consistent API URL across all development machines
- ? Frontend can use a single, hardcoded API base URL
- ? No need to update frontend configuration when switching machines
- ? Easier to share and collaborate on the project

## Changing Ports

If you need to change the ports, update all three configuration files mentioned above.
