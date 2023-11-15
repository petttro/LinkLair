using System;
using LinkLair.Common.Exceptions;
using Xunit;

namespace LinkLair.Api.Test.Exceptions;

public class ExceptionTests
{
    [Fact]
    public void InternalSystemException_Constructor_Success()
    {
        var content = new InternalSystemException();
        Assert.NotNull(content);

        var errorMessage = "Error Message";
        content = new InternalSystemException(errorMessage);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);

        var exception = new Exception();
        content = new InternalSystemException(errorMessage, exception);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);
        Assert.Equal(content.InnerException, exception);

        Assert.Null(content.CustomOverrideMessage);
        content.CustomOverrideMessage = errorMessage;

        Assert.Equal(errorMessage, content.CustomOverrideMessage);
    }

    [Fact]
    public void UnauthorizedAccessException_Constructor_Success()
    {
        var content = new UnauthorizedAccessException();
        Assert.NotNull(content);

        var errorMessage = "Error Message";
        content = new UnauthorizedAccessException(errorMessage);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);

        var exception = new Exception();
        content = new UnauthorizedAccessException(errorMessage, exception);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);
        Assert.Equal(content.InnerException, exception);
    }

    [Fact]
    public void UnauthorizedException_Constructor_Success()
    {
        var customCode = CustomErrorCode.ItemAlreadyExistsDefault;

        var content = new UnauthorizedException();
        Assert.NotNull(content);

        content = new UnauthorizedException(customCode);
        Assert.NotNull(content);
        Assert.Equal(customCode, content.CustomErrorCode);

        var errorMessage = "Error Message";
        content = new UnauthorizedException(errorMessage);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);

        content = new UnauthorizedException(errorMessage, customCode);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);
        Assert.Equal(customCode, content.CustomErrorCode);

        var exception = new Exception();
        content = new UnauthorizedException(errorMessage, exception);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);
        Assert.Equal(content.InnerException, exception);

        content = new UnauthorizedException(errorMessage, exception, customCode);
        Assert.NotNull(content);
        Assert.Equal(content.Message, errorMessage);
        Assert.Equal(content.InnerException, exception);
        Assert.Equal(customCode, content.CustomErrorCode);
    }
}
