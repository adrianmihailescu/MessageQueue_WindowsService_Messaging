USE [master]
GO
/****** Object:  Database [TEST_PRINTEC]    Script Date: 12/15/2011 23:56:28 ******/
CREATE DATABASE [TEST_PRINTEC] ON  PRIMARY 
( NAME = N'TestPrintec', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL.2\MSSQL\DATA\TestPrintec.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TestPrintec_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL.2\MSSQL\DATA\TestPrintec_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Romanian_CI_AS
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'TEST_PRINTEC', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TEST_PRINTEC].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [TEST_PRINTEC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET ARITHABORT OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [TEST_PRINTEC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TEST_PRINTEC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TEST_PRINTEC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TEST_PRINTEC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TEST_PRINTEC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TEST_PRINTEC] SET  READ_WRITE 
GO
ALTER DATABASE [TEST_PRINTEC] SET RECOVERY FULL 
GO
ALTER DATABASE [TEST_PRINTEC] SET  MULTI_USER 
GO
ALTER DATABASE [TEST_PRINTEC] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TEST_PRINTEC] SET DB_CHAINING OFF 