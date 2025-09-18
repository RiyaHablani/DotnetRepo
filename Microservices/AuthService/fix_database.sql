-- Script to reset the database and migration history
-- Run this in MySQL Workbench or command line

-- Drop the database if it exists and recreate it
DROP DATABASE IF EXISTS HospitalManagementDB;
CREATE DATABASE HospitalManagementDB;
USE HospitalManagementDB;

-- The tables will be created by Entity Framework migrations
-- This just ensures we have a clean database to work with
