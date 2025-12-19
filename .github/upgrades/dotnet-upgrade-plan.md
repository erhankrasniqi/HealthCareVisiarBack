# .NET 9.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade CQRS-Decorator.SharedKernel\CQRS-Decorator.SharedKernel.csproj
4. Upgrade CQRS-Decorator.Domain\CQRS-Decorator.Domain.csproj
5. Upgrade CQRS-Decorator.Application\CQRS-Decorator.Application.csproj
6. Upgrade CQRS-Decorator.Infrastructure\CQRS-Decorator.Infrastructure.csproj
7. Upgrade CQRS-Decorator.Decorators\CQRS-Decorator.Decorators.csproj
8. Upgrade CQRS-Decorator.Tests\CQRS-Decorator.Tests.csproj
9. Upgrade CQRS-Decorator.API\CQRS-Decorator.API.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                      | Current Version | New Version | Description                                   |
|:--------------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.AspNetCore.Authentication.JwtBearer     | 8.0.0           | 9.0.11      | Recommended for .NET 9.0                      |
| Microsoft.EntityFrameworkCore                     | 9.0.7           | 9.0.11      | Recommended for .NET 9.0                      |
| Microsoft.EntityFrameworkCore.Design              | 9.0.7           | 9.0.11      | Recommended for .NET 9.0                      |
| Microsoft.Extensions.Configuration.Abstractions   | 10.0.1          | 9.0.11      | Recommended for .NET 9.0                      |
| Microsoft.Extensions.Logging.Abstractions         | 9.0.7           | 9.0.11      | Recommended for .NET 9.0                      |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### CQRS-Decorator.SharedKernel\CQRS-Decorator.SharedKernel.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### CQRS-Decorator.Domain\CQRS-Decorator.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### CQRS-Decorator.Application\CQRS-Decorator.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### CQRS-Decorator.Infrastructure\CQRS-Decorator.Infrastructure.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

NuGet packages changes:
  - Microsoft.EntityFrameworkCore should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9.0*)
  - Microsoft.EntityFrameworkCore.Design should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9.0*)
  - Microsoft.Extensions.Configuration.Abstractions should be updated from `10.0.1` to `9.0.11` (*recommended for .NET 9.0*)

#### CQRS-Decorator.Decorators\CQRS-Decorator.Decorators.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

NuGet packages changes:
  - Microsoft.Extensions.Logging.Abstractions should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9.0*)

#### CQRS-Decorator.Tests\CQRS-Decorator.Tests.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### CQRS-Decorator.API\CQRS-Decorator.API.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Authentication.JwtBearer should be updated from `8.0.0` to `9.0.11` (*recommended for .NET 9.0*)
  - Microsoft.EntityFrameworkCore.Design should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9.0*)
