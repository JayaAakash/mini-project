create database MoviesDB

CREATE TABLE Movie (
    Mid INT PRIMARY KEY,
    Moviename NVARCHAR(255) NOT NULL,
    DateofRelease DATE
)

insert into Movie (Mid, Moviename, DateofRelease)

VALUES (1, 'Hunters', '06/09/2007'),
(2, 'wolf of tiger', '09/09/2000'),
(3, 'Money Heist', '11/02/2018')
