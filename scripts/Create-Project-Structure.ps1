#Requires -Version 5.1
<#
.SYNOPSIS
    Sets up or verifies the .NET project structure for the TTRPG AI Bot.
.DESCRIPTION
    Creates project directories if they don't exist, initializes .csproj files
    for .NET 9, adds required NuGet packages (including pre-release where specified),
    and sets up inter-project references.
    Designed to be run from the intended solution directory (e.g., 'TtrpgAiBot').
.PARAMETER TargetPath
    The path for the 'TtrpgAiBot' solution folder.
    Defaults to the current directory. If the directory doesn't exist, it will be created.
.EXAMPLE
    # Run from C:\Projects\TtrpgAiBot (will use or create this directory)
    .\Setup-TtrpgBotProjects.ps1

.EXAMPLE
    # Explicitly specify the path (will use or create C:\MyDev\TtrpgAiBot)
    .\Setup-TtrpgBotProjects.ps1 -TargetPath "C:\MyDev\TtrpgAiBot"
#>
param(
    [string]$TargetPath = (Get-Location).Path
)

# --- Configuration ---
$solutionName = "TtrpgAiBot" # Used for the .sln filename
$targetFramework = "net9.0"

# --- Determine Solution Directory ---
$solutionDir = $TargetPath
Write-Host "Ensuring solution directory exists: $solutionDir"
if (-not (Test-Path -Path $solutionDir -PathType Container)) {
    Write-Host "Creating solution directory: $solutionDir"
    New-Item -ItemType Directory -Path $solutionDir -Force | Out-Null
}

$srcDir = Join-Path -Path $solutionDir -ChildPath "src"
$solutionFile = Join-Path -Path $solutionDir -ChildPath "$solutionName.sln"

# Define Projects, their types, packages, and references
$projects = @(
    @{ Name = "TtrpgAiBot.Core"; Type = "classlib"; Packages = @("MediatR"); References = @() }
    @{ Name = "TtrpgAiBot.Domain"; Type = "classlib"; Packages = @(); References = @("TtrpgAiBot.Core") }
    @{ Name = "TtrpgAiBot.Application"; Type = "classlib"; Packages = @("MediatR", "FluentValidation", "AutoMapper"); References = @("TtrpgAiBot.Core", "TtrpgAiBot.Domain") }
    @{ Name = "TtrpgAiBot.Infrastructure"; Type = "classlib"; Packages = @("Marten", "Microsoft.Extensions.Caching.StackExchangeRedis", "Arch", "PdfPig", "System.Net.Http.Json", "Microsoft.ML.OnnxRuntime" ); References = @("TtrpgAiBot.Core", "TtrpgAiBot.Application") }
    @{ Name = "TtrpgAiBot.Api"; Type = "webapi"; Packages = @("Microsoft.AspNetCore.OpenApi", "Swashbuckle.AspNetCore", "MediatR.Extensions.Microsoft.DependencyInjection", "FluentValidation.DependencyInjectionExtensions", "AutoMapper.Extensions.Microsoft.DependencyInjection", "Marten", "Microsoft.AspNetCore.Mvc.Versioning", "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer"); References = @("TtrpgAiBot.Application", "TtrpgAiBot.Infrastructure", "TtrpgAiBot.Bot", "TtrpgAiBot.Platform.Discord", "TtrpgAiBot.Ai") }
    @{ Name = "TtrpgAiBot.Bot"; Type = "classlib"; Packages = @("Microsoft.Extensions.Hosting.Abstractions"); References = @("TtrpgAiBot.Core", "TtrpgAiBot.Platform.Discord") }
    # Specify NetCord packages as needing pre-release flag
    @{ Name = "TtrpgAiBot.Platform.Discord"; Type = "classlib"; Packages = @(
            @{ Name = "NetCord"; Prerelease = $true },
            @{ Name = "NetCord.Hosting.Services"; Prerelease = $true },
            @{ Name = "NetCord.Services"; Prerelease = $true },
            @{ Name = "MediatR"; Prerelease = $false } # Example of mixing
        ); References = @("TtrpgAiBot.Core", "TtrpgAiBot.Application") }
    @{ Name = "TtrpgAiBot.Ai"; Type = "classlib"; Packages = @("Microsoft.SemanticKernel", "Microsoft.SemanticKernel.Connectors.OpenAI", "Microsoft.Extensions.Http"); References = @("TtrpgAiBot.Core", "TtrpgAiBot.Application") }
    @{ Name = "TtrpgAiBot.GameSystems.Dnd5e"; Type = "classlib"; Packages = @("Arch"); References = @("TtrpgAiBot.Core") }
    @{ Name = "TtrpgAiBot.GameSystems.Shadowrun"; Type = "classlib"; Packages = @("Arch"); References = @("TtrpgAiBot.Core") }
)

# --- Helper Functions ---
function Invoke-DotNet($command, [string[]]$arguments, $workingDir, $errorMessage) {
    $displayArgs = $arguments -join ' '
    Write-Host "EXEC: dotnet $command $displayArgs (in $workingDir)"
    $currentLocation = Get-Location
    Set-Location $workingDir
    $output = & dotnet $command $arguments 2>&1
    $exitCode = $LASTEXITCODE
    Set-Location $currentLocation

    if ($exitCode -ne 0) {
        Write-Error "$errorMessage`nCommand: dotnet $command $displayArgs`nOutput: $output`nExit Code: $exitCode"
        throw "$errorMessage"
    } else {
        Write-Host "SUCCESS: dotnet $command completed."
    }
}

function Get-RelativePath {
    param(
        [string]$targetPath,
        [string]$basePath
    )
    $targetFullPath = (Resolve-Path -Path $targetPath).Path
    $baseFullPath = (Resolve-Path -Path $basePath).Path
    $targetUri = [System.Uri]$targetFullPath
    $baseUri = [System.Uri]$baseFullPath
    if (-not $baseUri.AbsoluteUri.EndsWith('/')) { $baseUri = [System.Uri]($baseUri.AbsoluteUri + '/') }
    $relativeUri = $baseUri.MakeRelativeUri($targetUri)
    $relativePath = [System.Uri]::UnescapeDataString($relativeUri.OriginalString)
    return $relativePath
}

# --- Script Body ---
Write-Host "Verifying dotnet command..."
try { dotnet --version | Out-Null; Write-Host "Dotnet found: $((dotnet --version)[0])" } catch { Write-Error "Failed to execute 'dotnet --version'."; exit 1 }

Write-Host "Ensuring TTRPG AI Bot project structure in '$solutionDir'..."
if (-not (Test-Path -Path $solutionDir -PathType Container)) { Write-Host "Creating solution directory: $solutionDir"; New-Item -ItemType Directory -Path $solutionDir -Force | Out-Null } else { Write-Host "Solution directory already exists: $solutionDir" }
if (-not (Test-Path -Path $srcDir -PathType Container)) { Write-Host "Creating src directory: $srcDir"; New-Item -ItemType Directory -Path $srcDir -Force | Out-Null } else { Write-Host "Src directory already exists: $srcDir" }

if (-not (Test-Path -Path $solutionFile)) { Invoke-DotNet "new" @("sln", "-n", $solutionName) $solutionDir "Failed to create solution file." } else { Write-Host "Solution file already exists: $solutionFile" }

foreach ($project in $projects) {
    $projectName = $project.Name; $projectType = $project.Type; $projectDir = Join-Path -Path $srcDir -ChildPath $projectName; $projectFile = Join-Path -Path $projectDir -ChildPath "$projectName.csproj"
    Write-Host "`n--- Processing Project: $projectName ---"
    if (-not (Test-Path -Path $projectDir -PathType Container)) { Write-Host "Creating project directory: $projectDir"; New-Item -ItemType Directory -Path $projectDir -Force | Out-Null }
    if (-not (Test-Path -Path $projectFile)) { $newProjectArgs = @($projectType, "-n", $projectName, "-f", $targetFramework, "--output", $projectDir, "--force"); Invoke-DotNet "new" $newProjectArgs $solutionDir "Failed to create project $projectName." } else { Write-Host "Project file already exists: $projectFile" }
    $slnContent = Get-Content $solutionFile -Raw; $projectFileRelativeForSlnCheck = Get-RelativePath -targetPath $projectFile -basePath $solutionDir
    if ($slnContent -notmatch [regex]::Escape($projectFileRelativeForSlnCheck.Replace('\','\\'))) { Invoke-DotNet "sln" @($solutionFile, "add", $projectFile) $solutionDir "Failed to add $projectName to solution." } else { Write-Host "$projectName already in solution." }

    # Add NuGet packages
    foreach ($packageInfo in $project.Packages) {
        $packageName = ""
        $needsPrerelease = $false

        # Handle both simple strings and hashtables for package definitions
        if ($packageInfo -is [string]) {
            $packageName = $packageInfo
        } elseif ($packageInfo -is [hashtable]) {
            $packageName = $packageInfo.Name
            # Check if Prerelease key exists and is true
            if ($packageInfo.ContainsKey('Prerelease') -and $packageInfo.Prerelease -eq $true) {
                $needsPrerelease = $true
            }
        } else {
            Write-Warning "Skipping invalid package definition for project ${projectName}: $packageInfo"
            continue
        }

        # Construct arguments for dotnet add package
        $addPackageArgs = @($projectFile, "package", $packageName)
        if ($needsPrerelease) {
            $addPackageArgs += "--prerelease" # Add the flag if needed
        }

        Invoke-DotNet "add" $addPackageArgs $projectDir "Failed to add package $packageName to $projectName."
    }
}

# Add Project References
Write-Host "`n--- Adding Project References ---"
foreach ($project in $projects) {
    $projectName = $project.Name; $projectDir = Join-Path -Path $srcDir -ChildPath $projectName; $projectFile = Join-Path -Path $projectDir -ChildPath "$projectName.csproj"
    if ($project.References.Count -gt 0) {
        Write-Host "Checking references for $projectName..."
        foreach ($refName in $project.References) {
            $refProjectFileAbs = Join-Path -Path $srcDir -ChildPath "$refName\$refName.csproj"; if (-not (Test-Path -Path $refProjectFileAbs)) { Write-Error "Referenced project file not found: $refProjectFileAbs."; continue }
            $relativeRefPath = Get-RelativePath -targetPath $refProjectFileAbs -basePath $projectDir
            $projContent = Get-Content $projectFile -Raw; $escapedRefPathForRegex = $relativeRefPath.Replace('\', '\\').Replace('.', '\.')
            if ($projContent -notmatch [regex]::Escape("<ProjectReference Include=""$escapedRefPathForRegex""")) { Invoke-DotNet "add" @($projectFile, "reference", $relativeRefPath) $projectDir "Failed to add reference from $projectName to $refName." } else { Write-Host "Reference from $projectName to $refName already exists." }
        }
    }
}

Write-Host "`n--- Project setup/verification complete! ---"
Write-Host "Solution file: $solutionFile"
Write-Host "You should now be able to open the solution in your IDE."
Write-Host "*** NOTE: Embedding implementation using Microsoft.ML.OnnxRuntime needs specific model/tokenizer setup in TtrpgAiBot.Infrastructure. ***"

