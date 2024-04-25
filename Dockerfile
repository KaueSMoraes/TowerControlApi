FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

COPY . /usr/local/bin/

WORKDIR /usr/local/bin/AssemblyMaster

RUN dotnet publish -c Release -o .

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /usr/local/bin/AssemblyMaster .

RUN cp privateauth /home/privateauth

RUN cp terraform   /home/privatekey

USER $APP_UID

CMD ["./AssemblyMaster"]