# Merch Api
Сервис учета выдачи мерча сотрудникам. Контролирует кому, сколько и какой мерч выдан.

**Содержание**
1. [Как запустить](#how_to_run)
2. [Web Api](#Web Api)
3. [gRPC](#gRPC)
4. [Swagger](#swagger)
## <a id="how_to_run"></a>Как запустить
Прежде чем запустить приложение необходимо убедиться, что есть доступ к следующим:

* [stock-api](https://github.com/ozon-edu-csharp-2021/stock-api/pkgs/container/stock-api)
* [emailing-service](https://github.com/ozon-edu-csharp-2021/emailing-service/pkgs/container/emailing-service)
* [employees-service](https://github.com/ozon-edu-csharp-2021/employees-service/pkgs/container/employees-service)
* [supply-service](https://github.com/ozon-edu-csharp-2021/supply-service/pkgs/container/supply-service)

Чтобы запустить приложение, необходимо в корневой дирекории выполнить команды:
```console
docker-compose up
docker-compose up -d
```
## Web Api
По умолчанию Web api доступен на порту **8080**

## gRPC
По умолчанию gRPC-сервис доступен на порту **8081**

## Swagger
Swagger для web Merch Api доступно по адресу:
http://localhost:8080/swagger/index.html



