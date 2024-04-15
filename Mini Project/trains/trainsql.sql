create database Trains

use Trains

CREATE TABLE Trains (
    TrainNo INT,
    TrainName VARCHAR(100),
    Class VARCHAR(50),
    TotalBirth INT,
    AvailableBirth INT,
    FromStation VARCHAR(100),
    ToStation VARCHAR(100),
    Price DECIMAL(10, 2)
)

select * from trains


-- Inserting data into the Train table
INSERT INTO Trains (TrainNo, TrainName, Class, TotalBirth, AvailableBirth, FromStation, ToStation, Price)
VALUES
    (12760, 'charminar', 'first class', 110, 110, 'chennai', 'hyderbad', 1000),
    (20677, 'vande bharat', 'first class', 100, 100, 'banglore', 'hyderbad', 1000),   
    (12009, 'Shatabdi', 'first class', 80, 80, 'delhi', 'hyderbad', 3000),
	------------------------------------------------------------------------
	(12760, 'charminar', 'second class', 100, 100, 'chennai', 'hyderbad', 900),
    (20677, 'vande bharat', 'second class', 100, 100, 'banglore', 'hyderbad', 900),   
    (12009, 'Shatabdi', 'second class', 80, 80, 'delhi', 'hyderbad', 2000),
	-------------------------------------------------------------------------
	(12760, 'charminar', 'third class', 100, 100, 'chennai', 'hyderbad', 700),
    (20677, 'vande bharat', 'third class', 100, 100, 'banglore', 'hyderbad', 700),   
    (12009, 'Shatabdi', 'third class', 120, 120, 'delhi', 'hyderbad', 1500)
    
