# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# copying source code to cointainer folder and restoring .NET project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY MW-WebApp-Nuget.csproj .
RUN dotnet restore MW-WebApp-Nuget.csproj
COPY . .
WORKDIR /src

# The -c option specifies the build configuration
# The -v option specifies the verbosity level of the build output. 
# 2 refers to the file descriptor for standard error.
# > is the redirection operator.
# &1 means that standard error should be redirected to wherever standard output (stdout) is going.
RUN dotnet build -v normal -c Release -o /app/build > build_output.txt 2>&1

# building .NET Project inside docker container
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish 

# copying build output and publish code to /app directory.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/build_output.txt .

CMD dotnet MW-WebApp-Nuget.dll