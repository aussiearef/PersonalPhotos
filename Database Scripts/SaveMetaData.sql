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