USE [CrudWithSP]
GO
/****** Object:  Table [dbo].[tblEmployee]    Script Date: 29/11/2021 23:03:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmployee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[City] [varchar](20) NOT NULL,
	[Department] [varchar](20) NOT NULL,
	[Gender] [varchar](8) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spEmployee]    Script Date: 29/11/2021 23:03:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee]
(
	@EmployeeId INT = NULL,
	@Name VARCHAR(20) = NULL,
	@City VARCHAR(20) = NULL,
	@Department VARCHAR(20) = NULL,
	@Gender VARCHAR(8) = NULL,
	@ActionType VARCHAR(25)
)
AS
BEGIN
	if @ActionType = 'SaveData'
	BEGIN
		if NOT EXISTS(SELECT * FROM tblEmployee WHERE EmployeeId = @EmployeeId)
		BEGIN
			INSERT INTO tblEmployee (Name, City, Department, Gender) VALUES (@Name, @City, @Department, @Gender)
		END
		ELSE
		BEGIN
			UPDATE tblEmployee SET Name = @Name, City = @City, Department = @Department, Gender = @Gender WHERE EmployeeId = @EmployeeId
		END
	END
	if @ActionType = 'DeleteData'
	BEGIN
		DELETE tblEmployee WHERE EmployeeId = @EmployeeId
	END
	if @ActionType = 'FetchData'
	BEGIN
		SELECT EmployeeId AS EmpId, Name, City, Department, Gender FROM tblEmployee
	END
	if @ActionType = 'FetchRecord'
	BEGIN
		SELECT EmployeeId AS EmpId, Name, City, Department, Gender FROM tblEmployee WHERE EmployeeId = @EmployeeId
	END
END
GO
