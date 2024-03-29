﻿using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace WCF;

[ServiceContract]
[OpenApiBasePath("/api")]
internal interface IWebApi
{
    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/body")]
    [OpenApiTag("Tag")]
    [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(MessageContract))]
    void GetMessage(
        [OpenApiParameter(ContentTypes = new[] { "application/json", "text/xml" }, Description = "param description.")] MessageContract param);
}
