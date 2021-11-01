FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/OzonEdu.MerchApi/OzonEdu.MerchApi.csproj", "OzonEdu.MerchApi/"]
COPY ["src/OzonEdu.MerchApi.Infrastructure/OzonEdu.MerchApi.Infrastructure.csproj", "OzonEdu.MerchApi.Infrastructure/"]
COPY ["src/OzonEdu.MerchApi.Grpc/OzonEdu.MerchApi.Grpc.csproj", "OzonEdu.MerchApi.Grpc/"]
COPY ["src/OzonEdu.MerchApi.HttpModels/OzonEdu.MerchApi.HttpModels.csproj", "OzonEdu.MerchApi.HttpModels/"]

RUN dotnet restore "OzonEdu.MerchApi/OzonEdu.MerchApi.csproj"
COPY ["src/","."]
RUN dotnet build "OzonEdu.MerchApi/OzonEdu.MerchApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OzonEdu.MerchApi/OzonEdu.MerchApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OzonEdu.MerchApi.dll"]
