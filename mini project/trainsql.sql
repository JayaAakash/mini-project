create database Trains
USE Trains


CREATE TABLE Trains (
    TrainId INT PRIMARY KEY ,
    TrainName VARCHAR(100) NOT NULL,
    FirstClassBerths INT,
    SecondClassBerths INT,
    SleeperBerths INT,
    Source VARCHAR(100) NOT NULL,
    Destination VARCHAR(100) NOT NULL
)
ALTER TABLE Trains
ADD Price DECIMAL(10, 2)

ALTER TABLE Trains
ADD IsActive BIT NOT NULL DEFAULT 1

DELETE FROM Trains

INSERT INTO Trains (TrainId, TrainName, Source, Destination, FirstClassBerths, SecondClassBerths, SleeperBerths, Price)
VALUES

(12760, 'charminar', 'Chennai', 'hyderbad',50,100,200, 2000),
    (20677, 'vande bharat', 'banglore', 'hyderbad',50,100,200, 1500),   
    (12009, 'Shatabdi', 'delhi', 'hyderbad',50,100,200, 3000);

update Trains 
set FirstClassBerths=100,
	 secondclassBERTHS=100,
	 SleeperBerths=100
	 where TrainId in (12008,12213,12216,12301,97432)

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(50) NOT NULL
)
insert into Users (Username,Password)
values('','')


CREATE TABLE Admins (
    AdminId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(50) NOT NULL,
)

INSERT INTO Admins (Username,Password) values ('','')
delete from Admins where AdminId in (1,3)


CREATE TABLE Bookings (
    BookingId INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    TrainId INT NOT NULL,
    ClassType NVARCHAR(20) NOT NULL,
    NumTickets INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (TrainId) REFERENCES Trains(TrainId)
)

select * from Users
select * from Admins
select * from Trains
select * from Bookings

insert into Admins(Username,Password) values ('','')