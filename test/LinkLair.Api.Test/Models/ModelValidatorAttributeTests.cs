using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using LinkLair.Api.Attributes;
using LinkLair.Common.Exceptions;
using LinkLair.Common.Test;
using Xunit;

namespace LinkLair.Api.Test.Models;

public class ModelValidatorAttributeTests
{
    [Fact]
    public void ModelValidatorAttribute_OnActionExecuting_NullParameterThrowsNullReferenceException()
    {
        var attribute = new ModelValidatorAttribute();

        ActionExecutingContext context = null;

        Assert.Throws<NullReferenceException>(() => attribute.OnActionExecuting(context));
    }

    [Fact]
    public void ModelValidatorAttribute_SuccessWithNullResultAndValid()
    {
        var attribute = new ModelValidatorAttribute();

        ActionContext actionContext = new ActionContext()
        {
            ActionDescriptor = new ActionDescriptor(),
            ModelState = { },
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData()
        };
        IList<IFilterMetadata> filters = new List<IFilterMetadata>();
        IDictionary<string, object> actionArguments = new Dictionary<string, object>();
        object controller = new object();

        ActionExecutingContext context = new ActionExecutingContext(actionContext, filters, actionArguments, controller);

        attribute.OnActionExecuting(context);

        Assert.NotNull(attribute);
        Assert.NotNull(context.ModelState);
        Assert.True(context.ModelState.IsValid);
        Assert.Null(context.Result);
    }

    [Fact]
    public void ModelValidatorAttribute_NullThrowBadUserInputException()
    {
        var attribute = new ModelValidatorAttribute();

        HttpContext httpContext = new TestHttpContext();
        RouteData routeData = new RouteData();
        ActionDescriptor actionDescriptor = new ActionDescriptor();
        ModelStateDictionary modelState = new ModelStateDictionary();
        ActionContext actionContext = new ActionContext(httpContext, routeData, actionDescriptor, modelState);
        IList<IFilterMetadata> filters = new List<IFilterMetadata>();
        IDictionary<string, object> actionArguments = new Dictionary<string, object>() { { "ResourceKey", null } };
        var context = new ActionExecutingContext(actionContext, filters, actionArguments, null);

        Assert.Throws<BaseInputException>(() => attribute.OnActionExecuting(context));
    }

    [Fact]
    public void ModelValidatorAttribute_ThrowBadUserInputException()
    {
        var attribute = new ModelValidatorAttribute();

        HttpContext httpContext = new TestHttpContext();
        RouteData routeData = new RouteData();
        ActionDescriptor actionDescriptor = new ActionDescriptor();
        ModelStateDictionary modelState = new ModelStateDictionary();
        modelState.AddModelError("error", "Error message");
        ActionContext actionContext = new ActionContext(httpContext, routeData, actionDescriptor, modelState);
        IList<IFilterMetadata> filters = new List<IFilterMetadata>();
        IDictionary<string, object> actionArguments = new Dictionary<string, object>();
        var context = new ActionExecutingContext(actionContext, filters, actionArguments, null);

        Assert.Throws<BaseInputException>(() => attribute.OnActionExecuting(context));
    }
}
