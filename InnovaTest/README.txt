For this project to work, you must create this table in a MySQL database of your choice

CREATE TABLE People (
    Id INT PRIMARY KEY auto_increment,
    FullName NVARCHAR(150) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Email NVARCHAR(150) NOT NULL,
    ImagePath NVARCHAR(255) NULL
);

change the line in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=127.0.0.1;port=3306;Database=peopledb;User=root;Password=Aq!2025@1984;" // change this line here
}