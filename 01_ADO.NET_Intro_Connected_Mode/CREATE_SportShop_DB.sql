CREATE DATABASE SportShop
COLLATE Cyrillic_General_100_CI_AS;
GO

USE SportShop;
GO

--drop DATABASE SportShop;

-- Товари: назва товару, вид товару (одяг, взуття, і т.д.), кількість товару в наявності, собівартість, виробник, ціна продажу
create table Products
(
	Id int identity(1,1) NOT NULL primary key,
	Name nvarchar(100) NOT NULL,
	TypeProduct nvarchar(20) NOT NULL,
	Quantity int NOT NULL,
	CostPrice int NOT NULL,
	Producer nvarchar(50),
	Price int NOT NULL
);
GO

INSERT INTO Products(Name, TypeProduct, Quantity, CostPrice, Producer, Price)
VALUES ('Рукавиці', 'Аксесуари', 5, 85, 'Туреччина', 150),
		('Окуляри', 'Аксесуари', 5, 85, 'Китай', 150),
		('Ремінь', 'Одяг', 15, 120, 'Туреччина', 250),
		('Рюкзак', 'Аксесуари', 10, 450, 'Польща', 700),
		('Кросівки Адідас', 'Взуття', 20, 800, 'Польща', 1500)
GO

select * from Products

-- Працівники: повне ім'я, дата прийняття на роботу, стать, ЗП
CREATE TABLE Employees
(
	Id INT Identity(1, 1) NOT NULL PRIMARY KEY,
	FullName NVarchar(100) NOT NULL, Check(LEN(FullName) > 0),
	HireDate Date NOT NULL,
	Gender NVarchar(1)NOT NULL,
	Salary Money NOT NULL, Check(Salary > 0)
);
Go

INSERT INTO Employees(FullName, HireDate, Gender, Salary)
VALUES ('Ярощук Іван Петрович', '2020-05-30', 'M', 8500),
('Михальчук Руслана Романівна', '2020-05-06', 'F', 8500),
('Левчук Тетяна Степанівна', '2020-05-07', 'F', 8500),
('Волос Ігор Іванович', '2020-05-15', 'M', 8500);
GO

-- Клієнти: повне ім'я, пошта, телефон, стать, знижка, підписка
CREATE TABLE Clients
(
	Id INT Identity(1, 1) NOT NULL PRIMARY KEY,
	FullName NVarchar(100) NOT NULL,
	Email NVarchar(100) NOT NULL,
	Phone NVarchar(15) NOT NULL,
	Gender NVarchar(1)NOT NULL,
	PercentSale INT NOT NULL CHECK(PercentSale >=0 AND PercentSale <=100) Default 0,
	Subscribe BIT Default 1
);
GO

INSERT INTO Clients(FullName, Email, Phone, Gender, PercentSale, Subscribe)
VALUES ('Петрук Степан Романович', 'ss@c.com', '0971234567', 'M', 10, 0),
('Романчук Людмила Степанівна', 'rls@rr.org', '0989876543', 'F', 15, 1)
GO

-- Продажі: ціна продажі, кількість одениць товару, товар, клієнт (який виконав покупку), працівник (який виконав продажу)
CREATE TABLE Salles
(
    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ProductId INT REFERENCES Products(Id) NOT NULL,
    Price Money NOT NULL,
    Quantity INT NOT NULL,
    EmployeeId INT REFERENCES Employees(Id) NOT NULL,
    ClientId INT REFERENCES Clients(Id) NOT NULL,
    BuyDate date NOT NULL
);
GO

INSERT INTO Salles(ProductId, Price, Quantity, EmployeeId, ClientId, BuyDate)
VALUES  (1, 10000, 1, 1, 1, '2024-03-15'),
        (1, 100,   1, 1, 1, '2025-02-11'),
        (4, 1800,  1, 2, 2, '2023-12-05'),
        (2, 10000, 3, 4, 2, '2024-09-01');
GO


-- вивід значень з таблиць
SELECT * FROM Products;
SELECT * FROM Employees;
SELECT * FROM Clients;
SELECT * FROM Salles;


