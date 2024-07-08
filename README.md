# Project for Introduction to Software Engineering course

## Requirements

- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://docs.docker.com/engine/install/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## How To Run

1. Go into the root directory of the project
1. Edit the **.env** file and input your parameters for database credentials etc.
1. Run `docker compose up -d`
1. `cd` into 'LangLang' folder
1. Run the following commands to install the **dotnet-ef** tool and update the database
    ```bash
    dotnet tool install -g dotnet-ef
    dotnet ef database update
    ```
1. Run the project with `dotnet run`

### Contributors:
- [Damjan Vinčić](https://github.com/DamjanVincic)
- [Nađa Zorić](https://github.com/zoricnadja)
- [Milica Radić](https://github.com/milicaradicc)
- [Mijat Krivokapić](https://github.com/mijatkrivokapic)
