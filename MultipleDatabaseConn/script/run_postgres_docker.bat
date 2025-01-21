@echo off
echo Starting PostgreSQL Docker containers...

:: Migration DB (用於產生遷移檔案)
docker run -d --name postgres_migration -p 5431:5432 -e POSTGRES_PASSWORD=Migration -e POSTGRES_USER=migration_user -e POSTGRES_DB=migration_db postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_migration

:: X00
docker run -d --name postgres_x00 -p 5432:5432 -e POSTGRES_PASSWORD=Default -e POSTGRES_USER=user_x00 -e POSTGRES_DB=db_x00 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x00

:: X01
docker run -d --name postgres_x01 -p 5433:5432 -e POSTGRES_PASSWORD=Second -e POSTGRES_USER=user_x01 -e POSTGRES_DB=db_x01 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x01

:: X02
docker run -d --name postgres_x02 -p 5434:5432 -e POSTGRES_PASSWORD=Third -e POSTGRES_USER=user_x02 -e POSTGRES_DB=db_x02 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x02

:: X03
docker run -d --name postgres_x03 -p 5435:5432 -e POSTGRES_PASSWORD=Fourth -e POSTGRES_USER=user_x03 -e POSTGRES_DB=db_x03 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x03

:: X04
docker run -d --name postgres_x04 -p 5436:5432 -e POSTGRES_PASSWORD=Fifth -e POSTGRES_USER=user_x04 -e POSTGRES_DB=db_x04 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x04

:: X05
docker run -d --name postgres_x05 -p 5437:5432 -e POSTGRES_PASSWORD=Sixth -e POSTGRES_USER=user_x05 -e POSTGRES_DB=db_x05 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x05

:: X06
docker run -d --name postgres_x06 -p 5438:5432 -e POSTGRES_PASSWORD=Seventh -e POSTGRES_USER=user_x06 -e POSTGRES_DB=db_x06 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x06

:: X07
docker run -d --name postgres_x07 -p 5439:5432 -e POSTGRES_PASSWORD=Eighth -e POSTGRES_USER=user_x07 -e POSTGRES_DB=db_x07 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x07

:: X08
docker run -d --name postgres_x08 -p 5440:5432 -e POSTGRES_PASSWORD=Ninth -e POSTGRES_USER=user_x08 -e POSTGRES_DB=db_x08 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x08

:: X09
docker run -d --name postgres_x09 -p 5441:5432 -e POSTGRES_PASSWORD=Tenth -e POSTGRES_USER=user_x09 -e POSTGRES_DB=db_x09 postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x09

:: X0A
docker run -d --name postgres_x0a -p 5442:5432 -e POSTGRES_PASSWORD=Eleventh -e POSTGRES_USER=user_x0a -e POSTGRES_DB=db_x0a postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0a

:: X0B
docker run -d --name postgres_x0b -p 5443:5432 -e POSTGRES_PASSWORD=Twelfth -e POSTGRES_USER=user_x0b -e POSTGRES_DB=db_x0b postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0b

:: X0C
docker run -d --name postgres_x0c -p 5444:5432 -e POSTGRES_PASSWORD=Thirteenth -e POSTGRES_USER=user_x0c -e POSTGRES_DB=db_x0c postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0c

:: X0D
docker run -d --name postgres_x0d -p 5445:5432 -e POSTGRES_PASSWORD=Fourteenth -e POSTGRES_USER=user_x0d -e POSTGRES_DB=db_x0d postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0d

:: X0E
docker run -d --name postgres_x0e -p 5446:5432 -e POSTGRES_PASSWORD=Fifteenth -e POSTGRES_USER=user_x0e -e POSTGRES_DB=db_x0e postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0e

:: X0F
docker run -d --name postgres_x0f -p 5447:5432 -e POSTGRES_PASSWORD=Sixteenth -e POSTGRES_USER=user_x0f -e POSTGRES_DB=db_x0f postgres:latest
if %ERRORLEVEL% neq 0 echo Error starting postgres_x0f

echo All PostgreSQL containers have been started.
echo Use 'docker ps' to check running containers.
pause