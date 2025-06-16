CREATE TABLE Sauces (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    IsVegan BIT
);

CREATE TABLE Toppings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Calories INT
);

CREATE TABLE Beverages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Price DECIMAL(5,2)
);

CREATE TABLE Pizzas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    SauceId INT FOREIGN KEY REFERENCES Sauces(Id),
    Price DECIMAL(5,2)
);

CREATE TABLE PizzaTopping (
    PizzaId INT FOREIGN KEY REFERENCES Pizzas(Id),
    ToppingId INT FOREIGN KEY REFERENCES Toppings(Id),
    PRIMARY KEY (PizzaId, ToppingId)
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
	"Name" NVARCHAR(255) NOT NULL DEFAULT ''
);

CREATE TABLE PizzaOrders (
    OrderId INT NOT NULL,
    PizzaId INT NOT NULL,
    PRIMARY KEY (OrderId, PizzaId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (PizzaId) REFERENCES Pizzas(Id) ON DELETE CASCADE
);

CREATE TABLE OrderBeverages (
    OrderId INT NOT NULL,
    BeverageId INT NOT NULL,
    PRIMARY KEY (OrderId, BeverageId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (BeverageId) REFERENCES Beverages(Id) ON DELETE CASCADE
);

SET IDENTITY_INSERT Sauces ON;
INSERT INTO Sauces (Id, Name, IsVegan) VALUES (1, 'Salsa di pomodoro', 1);
INSERT INTO Sauces (Id, Name, IsVegan) VALUES (2, 'Base olio di oliva', 1);
SET IDENTITY_INSERT Sauces OFF;

SET IDENTITY_INSERT Toppings ON;
INSERT INTO Toppings (Id, Name, Calories) VALUES (1, 'Mozzarella di Bufala', 85);
INSERT INTO Toppings (Id, Name, Calories) VALUES (2, 'basilico', 1);
INSERT INTO Toppings (Id, Name, Calories) VALUES (3, 'Aglio', 4);
INSERT INTO Toppings (Id, Name, Calories) VALUES (4, 'Origano', 2);
INSERT INTO Toppings (Id, Name, Calories) VALUES (5, 'Acciughe', 40);
INSERT INTO Toppings (Id, Name, Calories) VALUES (6, 'Peperoni', 120);
INSERT INTO Toppings (Id, Name, Calories) VALUES (7, 'Prosciutto Crudo', 90);
INSERT INTO Toppings (Id, Name, Calories) VALUES (8, 'Funghi', 15);
INSERT INTO Toppings (Id, Name, Calories) VALUES (9, 'Rucola', 5);
INSERT INTO Toppings (Id, Name, Calories) VALUES (10, 'Parmigiano Reggiano', 110);
SET IDENTITY_INSERT Toppings OFF;

SET IDENTITY_INSERT Beverages ON;
INSERT INTO Beverages (Id, Name, Price) VALUES (1, 'Birra Chiara', 3.50);
INSERT INTO Beverages (Id, Name, Price) VALUES (2, 'Birra Scura', 4.00);
INSERT INTO Beverages (Id, Name, Price) VALUES (3, 'Coca Cola', 2.50);
SET IDENTITY_INSERT Beverages OFF;

SET IDENTITY_INSERT Pizzas ON;
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (1, 'Margherita', 1, 7.50);
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (2, 'Marinara', 1, 6.00);
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (3, 'Diavola', 1, 8.50);
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (4, 'Prosciutto e Funghi', 1, 9.00);
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (5, 'Parmigiana', 1, 8.00);
INSERT INTO Pizzas (Id, Name, SauceId, Price) VALUES (6, 'Bianca con Rucola', 2, 8.50);
SET IDENTITY_INSERT Pizzas OFF;

INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (1, 1);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (1, 2);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (2, 3);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (2, 4);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (3, 1);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (3, 6);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (4, 1);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (4, 7);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (4, 8);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (5, 1);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (5, 10);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (5, 2);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (6, 1);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (6, 9);
INSERT INTO PizzaTopping (PizzaId, ToppingId) VALUES (6, 10);