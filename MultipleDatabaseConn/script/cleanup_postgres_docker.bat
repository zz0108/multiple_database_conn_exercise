@echo off
echo Stopping and removing PostgreSQL Docker containers...

:: 停止所有容器
docker stop postgres_migration postgres_x00 postgres_x01 postgres_x02 postgres_x03 postgres_x04 postgres_x05 postgres_x06 postgres_x07 postgres_x08 postgres_x09 postgres_x0a postgres_x0b postgres_x0c postgres_x0d postgres_x0e postgres_x0f

:: 移除所有容器
docker rm postgres_migration postgres_x00 postgres_x01 postgres_x02 postgres_x03 postgres_x04 postgres_x05 postgres_x06 postgres_x07 postgres_x08 postgres_x09 postgres_x0a postgres_x0b postgres_x0c postgres_x0d postgres_x0e postgres_x0f

echo Cleanup completed.
echo Use 'docker ps -a' to verify.
pause