CREATE TABLE Roles (
                       Id INT PRIMARY KEY ,
                       Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
                       Id INT PRIMARY KEY ,
                       Username NVARCHAR(50) NOT NULL,
                       Password NVARCHAR(255) NOT NULL,
                       RefreshToken NVARCHAR(255),
                       RefreshTokenExpire DATETIME,
                       RoleId INT NOT NULL,
                       CONSTRAINT FK_User_Role FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);