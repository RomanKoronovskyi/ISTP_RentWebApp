Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"
  config.vm.hostname = "aspnetapp"

  config.vm.network "forwarded_port", guest: 5000, host: 5000
  config.vm.network "forwarded_port", guest: 1433, host: 1433

  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 2
  end

  config.vm.provision "shell", inline: <<-SHELL
    sudo apt-get update -y
    sudo apt-get upgrade -y

    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-9.0

    curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
    sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list)"
    sudo apt-get update
    sudo apt-get install -y mssql-server

    sudo MSSQL_PID=express MSSQL_AGENT_ENABLED=true MSSQL_SA_PASSWORD="..." /opt/mssql/bin/mssql-conf -n setup accept-eula

    sudo systemctl start mssql-server
    sudo systemctl enable mssql-server

    sudo ACCEPT_EULA=Y apt-get install -y mssql-tools msodbcsql17 unixodbc-dev
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
    source ~/.bashrc

    sudo apt-get install -y git
    git clone https://github.com/RomanKoronovskyi/ISTP_RentWebApp.git /home/vagrant/ISTP_RentWebApp

    cd /home/vagrant/ISTP_RentWebApp

    dotnet tool install --global dotnet-ef
    echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
    export PATH="$PATH:$HOME/.dotnet/tools"

    dotnet restore
    dotnet build
    dotnet ef database update
    nohup dotnet run --urls http://0.0.0.0:5000 > log.txt 2>&1 &
  SHELL
end
