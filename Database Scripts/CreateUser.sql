Create Proc CreateUser
@Email varchar(100),
@Password varchar(100)
AS
	Insert Into Users (Email, [Password])
	values (@Email, @Password)
Go

