﻿using System.IO;
using System.Net;
using System.Text;
using Alba.Stubs;
using Baseline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Alba
{
    public static class HttpContextExtensions
    {
        public static void ContentType(this HttpRequest request, string mimeType)
        {
            request.ContentType = mimeType;
        }

        public static void ContentType(this HttpResponse response, string mimeType)
        {
            response.Headers["content-type"] = mimeType;
        }

        public static void RelativeUrl(this HttpContext context, string relativeUrl)
        {
            if (relativeUrl != null && relativeUrl.Contains("?"))
            {
                var parts = relativeUrl.Trim().Split('?');
                context.Request.Path = parts[0];

                if (parts[1].IsNotEmpty())
                {
                    var dict = QueryHelpers.ParseQuery(parts[1]);

                    var request = context.Request.As<StubHttpRequest>();

                    foreach (var pair in dict)
                    {
                        request.AddQueryString(pair.Key, pair.Value);
                    }
                }
                
            }
            else
            {
                context.Request.Path = relativeUrl;
            }

            
        }

        public static void Accepts(this HttpContext context, string mimeType)
        {
            context.Request.Headers["accept"] = mimeType;
        }

        public static void HttpMethod(this HttpContext context, string method)
        {
            context.Request.Method = method;
        }

        public static void StatusCode(this HttpContext context, int statusCode)
        {
            context.Response.StatusCode = statusCode;
        }

        public static void Write(this HttpResponse response, string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            response.Body.Write(bytes, 0, bytes.Length);
            response.Body.Flush();
        }

    }
}