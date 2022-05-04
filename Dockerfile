# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
COPY . . 
EXPOSE 5000/tcp
EXPOSE 5001/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
