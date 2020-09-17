# Instructions for setting up with the latest version of EventStoreDB.

The exercises in this course are using the latest version of event store that is currently only available from GitHub packages.
To get access to the EventStoreDB container, you need to get a personal access token from GitHub and use it to register a new container source: 
 * Navigate to GitHub and go to settings -> developer settings -> personal access tokens.
 * Create a new token that has the `read:packages` scope checked.
 * Enter the following command in powershell/bash:
 
```
docker login https://docker.pkg.github.com -u <username> -p <access-token>
```

To make use of the latest nuget packages you need to add the following Github package source:
```
https://nuget.pkg.github.com/eventstore/index.json
```

Again you will need to use your username and access token to authorise.
With Rider and Visual Studio, you can do this via a popup when you build the application. I
If running from the command line, you should modify your global nuget.config with the following:

```
<?xml version="1.0" encoding="utf-8"?> <configuration>
<packageSources>
<add key="github" value="https://nuget.pkg.github.com/eventstore/index.json" />..
  </packageSources>
  <packageSourceCredentials>
<github>
<add key="Username" value="<username" />
<add key="ClearTextPassword" value=“<access-token>" />
    </github>
  </packageSourceCredentials>
</configuration>
```
