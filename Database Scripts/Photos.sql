Create Table Photos
(
	Id int not null identity(1,1) primary key,
	UserId int not null,
	Description varchar(max),
	FileName varchar(100)
)

Go

Create Index IX_Photos_UserId On Photos(UserId)

Go

Alter Table Photos 
Add Constraint FK_Photos_Users Foreign Key 
(UserId) References [Users](Id)
ON DELETE CASCADE
Go