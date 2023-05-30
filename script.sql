USE [master]
GO
/****** Object:  Database [LabsAutomationSystem]    Script Date: 12.05.2023 12:44 ******/
CREATE DATABASE [LabsAutomationSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LabsAutomationSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LabsAutomationSystem.mdf' , SIZE = 139264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LabsAutomationSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LabsAutomationSystem_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LabsAutomationSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LabsAutomationSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LabsAutomationSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LabsAutomationSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LabsAutomationSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LabsAutomationSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [LabsAutomationSystem] SET  MULTI_USER 
GO
ALTER DATABASE [LabsAutomationSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LabsAutomationSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LabsAutomationSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LabsAutomationSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LabsAutomationSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LabsAutomationSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LabsAutomationSystem', N'ON'
GO
ALTER DATABASE [LabsAutomationSystem] SET QUERY_STORE = ON
GO
ALTER DATABASE [LabsAutomationSystem] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LabsAutomationSystem]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
	[Specialty] [nvarchar](25) NOT NULL,
	[Teacher] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Journal]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Journal](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[Student] [uniqueidentifier] NOT NULL,
	[File] [varbinary](max) NOT NULL,
	[Ext] [nvarchar](10) NOT NULL,
	[Mark] [int] NULL,
	[Lab] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materials]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materials](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [varchar](150) NOT NULL,
	[File] [varbinary](max) NOT NULL,
	[Ext] [nvarchar](10) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](25) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Group] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](25) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Theme_cons]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Theme_cons](
	[Id] [uniqueidentifier] NOT NULL,
	[Theme] [uniqueidentifier] NOT NULL,
	[Matrial] [uniqueidentifier] NOT NULL,
	[Owner] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 12.05.2023 12:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Themes](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD FOREIGN KEY([Teacher])
REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Journal]  WITH CHECK ADD FOREIGN KEY([Lab])
REFERENCES [dbo].[Materials] ([Id])
GO
ALTER TABLE [dbo].[Journal]  WITH CHECK ADD FOREIGN KEY([Student])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD FOREIGN KEY([Group])
REFERENCES [dbo].[Groups] ([Id])
GO
ALTER TABLE [dbo].[Theme_cons]  WITH CHECK ADD FOREIGN KEY([Matrial])
REFERENCES [dbo].[Materials] ([Id])
GO
ALTER TABLE [dbo].[Theme_cons]  WITH CHECK ADD FOREIGN KEY([Owner])
REFERENCES [dbo].[Teachers] ([Id])
GO
ALTER TABLE [dbo].[Theme_cons]  WITH CHECK ADD FOREIGN KEY([Theme])
REFERENCES [dbo].[Themes] ([Id])
GO
USE [master]
GO
ALTER DATABASE [LabsAutomationSystem] SET  READ_WRITE 
GO
