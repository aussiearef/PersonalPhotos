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