/********************************************************
* This script creates the database named SmallAnimalDB 
*********************************************************/
USE master;
GO

IF  DB_ID('SmallAnimalDB') IS NOT NULL
    DROP DATABASE SmallAnimalDB;
GO

CREATE DATABASE SmallAnimalDB
GO

USE SmallAnimalDB;

-- create the tables for the database
CREATE TABLE Animals (
  SpeciesID        INT            PRIMARY KEY   IDENTITY,
  SpeciesName      VARCHAR(255)   NOT NULL      UNIQUE
);


-- Insert data into the tables


-- Create a user named MGSUser

/*
GRANT SELECT, INSERT, UPDATE, DELETE
ON *
TO MGSUser@localhost
IDENTIFIED BY 'pa55word';
*/