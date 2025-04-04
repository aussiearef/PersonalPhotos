using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;

namespace PersonalPhotos.Test;

public class PhotosTests
{
    [Fact]
    public async Task Upload_GivenValidFilePath_RedturnsRedirectToDisplayAction()
    {
        // Arrange
        var fileStorage = new Mock<IFileStorage>();
        var keyGenerator = new Mock<IKeyGenerator>();
        var photoMetadata = new Mock<IPhotoMetaData>();
        var httpContetAccesstor = new Mock<IHttpContextAccessor>();
        var fromFile = new Mock<IFormFile>();

        var viewModel = new Mock<PhotoUploadViewModel>();
        //viewModel.SetupGet(x => x.File).Returns(fromFile.Object);
        viewModel.Object.File = fromFile.Object;

        var session = new Mock<ISession>();
        session.Setup(x => x.Set("User", It.IsAny<byte[]>()));

        var context = new Mock<HttpContext>();
        context.SetupGet(x => x.Session).Returns(session.Object);
        httpContetAccesstor.SetupGet(x => x.HttpContext).Returns(context.Object);


        // Act
        var controller = new PhotosController(keyGenerator.Object, httpContetAccesstor.Object, photoMetadata.Object,
            fileStorage.Object);

        var result = await controller.Upload(viewModel.Object) as RedirectToActionResult;

       
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Display", result.ActionName, ignoreCase:true);

    }
}