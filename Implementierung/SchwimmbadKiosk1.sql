CREATE DATABASE SchwimmbadKiosk1;

USE SchwimmbadKiosk1;


CREATE TABLE Benutzer (
    BenutzerID INT IDENTITY(1,1) PRIMARY KEY,
    Benutzername NVARCHAR(50) NOT NULL UNIQUE,
    Passwort NVARCHAR(100) NOT NULL,
    Rolle NVARCHAR(20) 
);

INSERT INTO Benutzer (Benutzername, Passwort, Rolle)
VALUES 
('admin', 'admin123', 'Admin'),
('mitarbeiter1', 'passwort1', 'Mitarbeiter');

CREATE TABLE Inventar (
    ArtikelID INT IDENTITY(1,1) PRIMARY KEY,
    Artikelname NVARCHAR(100) NOT NULL,
    Beschreibung NVARCHAR(255),
    Preis DECIMAL(10, 2) NOT NULL,
    Bestand INT NOT NULL,
    HinzugefügtAm DATETIME DEFAULT GETDATE()
);

INSERT INTO Inventar (Artikelname, Beschreibung, Preis, Bestand)
VALUES 
('Wasserflasche', '500ml Mineralwasser', 1.50, 100),
('Eis', 'Schokoladeneis am Stiel', 2.00, 50),
('Sandwich', 'Käse-Schinken-Sandwich', 3.50, 30);

CREATE TABLE Verkäufe (
    VerkaufID INT IDENTITY(1,1) PRIMARY KEY,
    ArtikelID INT NOT NULL FOREIGN KEY REFERENCES Inventar(ArtikelID),
    Menge INT NOT NULL,
    Gesamtpreis DECIMAL(10, 2) NOT NULL,
    VerkaufDatum DATETIME DEFAULT GETDATE()
);

INSERT INTO Verkäufe (ArtikelID, Menge, Gesamtpreis)
VALUES 
(1, 2, 3.00), 
(2, 1, 2.00); 


CREATE TABLE Berichte (
    BerichtID INT IDENTITY(1,1) PRIMARY KEY,
    BerichtTyp NVARCHAR(50) NOT NULL, 
    ErstelltAm DATETIME DEFAULT GETDATE(),
    Beschreibung NVARCHAR(255)
);








