FROM microsoft/dotnet:2.1-sdk-alpine AS builder
WORKDIR /source
COPY ./Hjerpbakk.Tfs.Alerts.sln .

COPY ./Hjerpbakk.Tfs.Alerts/*.csproj ./Hjerpbakk.Tfs.Alerts/
COPY ./Hjerpbakk.Tfs.Alerts.Tests/*.csproj ./Hjerpbakk.Tfs.Alerts.Tests/
RUN dotnet restore

COPY ./Hjerpbakk.Tfs.Alerts ./Hjerpbakk.Tfs.Alerts
COPY ./Hjerpbakk.Tfs.Alerts.Tests ./Hjerpbakk.Tfs.Alerts.Tests

RUN dotnet test "./Hjerpbakk.Tfs.Alerts.Tests/Hjerpbakk.Tfs.Alerts.Tests.csproj"
RUN dotnet publish "./Hjerpbakk.Tfs.Alerts/Hjerpbakk.Tfs.Alerts.csproj" --output "../dist" --configuration Release --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /source/dist .
EXPOSE 4321
ARG SlackAPIToken
ENV SlackAPIToken=$SlackAPIToken
ENTRYPOINT ["dotnet", "Hjerpbakk.Tfs.Alerts.dll"]

