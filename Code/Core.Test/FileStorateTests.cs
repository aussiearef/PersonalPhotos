using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Core.Test;

public class FileStorageTests
{
    [Fact]
    public async Task StoreFile_GivenCorrectFilePath_ReturnsTask()
    {
        // Arrange 
        var webHostEnvironment = Mock.Of<IWebHostEnvironment>();
        var fileOperations = new Mock<IFileOperations>();
        var fromFile = new Mock<IFormFile>();

        var localFileStorage = new LocalFileStorage(webHostEnvironment, fileOperations.Object);

        // Act
        await localFileStorage.StoreFile(fromFile.Object, It.IsAny<string>());

        //Assert
        fileOperations.Verify(fo=> fo.Combine(It.IsAny<string>() , It.IsAny<string>()), Times.AtLeastOnce , "Combine method must have been called at least once.");
        fileOperations.Verify(f=> f.DirectoryExists(It.IsAny<string>()), Times.AtLeastOnce , "DirectoryExists must be called at least once.");
        fromFile.Verify(f=> f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}