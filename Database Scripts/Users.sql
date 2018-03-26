Create Table Users
(
	Id int not null identity(1,1) primary key,
	Email varchar(100) not null,
	[password] varchar(100) not null
)


Create Index IX_Users_Email On Users (Email)