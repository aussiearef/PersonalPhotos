Create Proc GetUser
@Email varchar(100)
AS
	Select * from Users
	Where Email = @Email;
Go
