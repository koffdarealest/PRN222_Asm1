CREATE DATABASE Prn222asm1
USE prn222asm1
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);

CREATE TABLE EventCategories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(255) NOT NULL
);

CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description TEXT NULL,
    StartTime DATETIME NULL,
    EndTime DATETIME NULL,
    Location NVARCHAR(255) NULL,
    CategoryID INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES EventCategories(CategoryID)
);

CREATE TABLE Attendees (
    AttendeeID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    UserID INT NULL,
    Name NVARCHAR(255) NULL,
    Email NVARCHAR(255) NULL,
    RegistrationTime DATETIME NULL,
    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Insert data into EventCategories
INSERT INTO EventCategories (CategoryName) VALUES ('Technology');
INSERT INTO EventCategories (CategoryName) VALUES ('Business');
INSERT INTO EventCategories (CategoryName) VALUES ('Health');
INSERT INTO EventCategories (CategoryName) VALUES ('Education');

-- Insert data into Events
INSERT INTO Events (Title, Description, StartTime, EndTime, Location, CategoryID) 
VALUES ('Tech Conference 2025', 'Annual technology conference', '2025-06-10 09:00:00', '2025-06-10 17:00:00', 'New York Convention Center', 1);

INSERT INTO Events (Title, Description, StartTime, EndTime, Location, CategoryID) 
VALUES ('Startup Pitch Night', 'Networking and startup pitching event', '2025-07-15 18:00:00', '2025-07-15 22:00:00', 'San Francisco Hub', 2);

INSERT INTO Events (Title, Description, StartTime, EndTime, Location, CategoryID) 
VALUES ('Health & Wellness Expo', 'Explore the latest in health and wellness', '2025-08-20 10:00:00', '2025-08-20 16:00:00', 'Los Angeles Expo Center', 3);

INSERT INTO Events (Title, Description, StartTime, EndTime, Location, CategoryID) 
VALUES ('Education Innovation Summit', 'Discussing the future of education', '2025-09-05 09:00:00', '2025-09-05 17:00:00', 'Boston Conference Hall', 4);
