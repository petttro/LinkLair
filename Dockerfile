FROM mcr.microsoft.com/dotnet/sdk:6.0.403

WORKDIR /opt

# make directories for published app and sources
RUN mkdir -p /app /opt/src 
COPY NuGet.Config /opt/NuGet.Config
COPY stylecop.ruleset /opt/stylecop.ruleset
COPY src/ /opt/src

# publish the app
RUN dotnet publish -c Release -o /app /opt/src/LinkLair.Api

FROM mcr.microsoft.com/dotnet/aspnet:6.0.11

WORKDIR /app
# copying published app from the previous build step
COPY --from=0 /app .

EXPOSE 80/tcp

CMD ["/opt/splunkforwarder/run.sh"]
