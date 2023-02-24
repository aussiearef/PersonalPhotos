Create Table Users
(
	Id int not null identity(1,1) primary key,
	Email varchar(100) not null,
	[password] varchar(100) not null
)


Create Index IX_Users_Email On Users (Email)

GO

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

Create Proc SaveMetaData
	@UserName varchar(100),
	@Description varchar(max),
	@FileName varchar(100)
As

declare @userId int

select @userId = Id 
from Users
where LTRIM(rtrim(lower(email))) = ltrim(rtrim(lower(@UserName)))

if @userId is not null
begin
	insert into Photos
	(UserId, [Description], [FileName])
	values (@userId, @Description, @FileName)
end
Go

Create Proc GetUserPhotos
	@UserName varchar(100)
As
	Declare @UserId int
	Select @UserId = Id
	From Users
	Where lower(Email) = lower(@UserName);

	if (@UserId is not null)
	Begin
		Select * 
		From Photos	
		Where UserId = @UserId;
	End;
Go

Create Proc GetUser
@Email varchar(100)
AS
	Select * from Users
	Where Email = @Email;
Go

Create Proc CreateUser
@Email varchar(100),
@Password varchar(100)
AS
	Insert Into Users (Email, [Password])
	values (@Email, @Password)
Go

  insert into Users
  (Email , [Password])
  VALUES
  ('test@xunitcourse.com', '123')

  go
  