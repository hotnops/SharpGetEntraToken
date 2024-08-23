# SharpGetEntraToken

## Intro
This dotnet executable is intended to be C2 capable and will obtain an Access Token for the provided client and resource. It uses the MSAL framework to obtain a token using whatever auth mechanism has been established. This has been testing with an Entra Joined OS with a PRT on Windows 11.

## Usage
The msalruntime.dll file needs to be present on the bin path of the executable running this assembly. If not it, it will fail. Most OSs do not have this, so I have provided the DLLs for upload. Ensure that you use the correct arch and that it is in the binary path.

Running the executable:
```
SharpGetEntraToken.exe <client_id> <tenant_id> <scope>
```

Example:
```
SharpGetEntraToken.exe 1950a258-227b-4e31-a9cf-717495945fc2 00000000-0000-0000-0000-000000000000 https://graph.microsoft.com/.default
```