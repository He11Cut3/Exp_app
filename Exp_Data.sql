use Expenses_db
go

CREATE TABLE Expenses (
    ExpenseId INT PRIMARY KEY IDENTITY,
    Date DATE NOT NULL,
    CategoryId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Comment NVARCHAR(255),
    FOREIGN KEY (CategoryId) REFERENCES ExpenseCategories(CategoryId)
);
CREATE TABLE ExpenseCategories (
    CategoryId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO ExpenseCategories (Name) VALUES ('Продукты питания');
INSERT INTO ExpenseCategories (Name) VALUES ('Транспорт');
INSERT INTO ExpenseCategories (Name) VALUES ('Мобильная связь');
INSERT INTO ExpenseCategories (Name) VALUES ('Интернет');
INSERT INTO ExpenseCategories (Name) VALUES ('Развлечения');