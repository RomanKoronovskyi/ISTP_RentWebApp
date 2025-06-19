Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"
  config.vm.hostname = "aspnetapp"

  # Проброс портів
  config.vm.network "forwarded_port", guest: 5000, host: 5000
  config.vm.network "forwarded_port", guest: 1433, host: 1433

  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 2
  end

  config.vm.provision "shell", inline: <<-SHELL
    echo "Оновлення системи..."
    sudo apt-get update -y
    sudo apt-get upgrade -y

    echo "Встановлення .NET SDK 7.0..."
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-9.0

    echo "Встановлення SQL Server..."
    curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
    sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list)"
    sudo apt-get update
    sudo apt-get install -y mssql-server

    echo "Налаштування SQL Server..."
    sudo MSSQL_PID=express MSSQL_AGENT_ENABLED=true MSSQL_SA_PASSWORD="YourStrong!Passw0rd" /opt/mssql/bin/mssql-conf -n setup accept-eula

    echo "Запуск служби SQL Server..."
    sudo systemctl start mssql-server
    sudo systemctl enable mssql-server

    echo "Встановлення SQL tools + драйверів..."
    sudo ACCEPT_EULA=Y apt-get install -y mssql-tools msodbcsql17 unixodbc-dev
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
    source ~/.bashrc

    echo "Встановлення git..."
    sudo apt-get install -y git

    echo "Клонування репозиторію..."
    git clone https://github.com/RomanKoronovskyi/ISTP_RentWebApp.git /home/vagrant/ISTP_RentWebApp

    echo "Перехід до каталогу проекту..."
    cd /home/vagrant/ISTP_RentWebApp

    echo "Встановлення EF Tools..."
    dotnet tool install --global dotnet-ef
    echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
    export PATH="$PATH:$HOME/.dotnet/tools"

    echo "Встановлення залежностей..."
    dotnet restore
    dotnet build

    echo "Запуск міграції бази даних..."
    dotnet ef database update

    echo "Запуск ASP.NET застосунку..."
    nohup dotnet run --urls http://0.0.0.0:5000 > log.txt 2>&1 &
  SHELL
end
