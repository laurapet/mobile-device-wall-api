CREATE DATABASE "mobileDeviceWallDB";

create table Lending(
    LendingID int PRIMARY KEY,
    UserID int NOT NULL,
    DeviceID int NOT NULL,
    IsLongterm boolean
    );