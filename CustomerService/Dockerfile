FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME


COPY CustomerService/CustomerService.csproj $APP_HOME/CustomerService/
RUN  cd $APP_HOME/CustomerService && dotnet restore

COPY CustomerService/. $APP_HOME/CustomerService/
RUN cd $APP_HOME/CustomerService && dotnet build


FROM build AS publish
WORKDIR $APP_HOME/CustomerService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /opt/app/CustomerService/out ./
ENTRYPOINT ["dotnet", "CustomerService.dll"]