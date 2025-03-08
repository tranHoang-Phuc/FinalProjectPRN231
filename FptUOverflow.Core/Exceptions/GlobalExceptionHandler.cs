using FptUOverflow.Core.CoreObjects;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Core.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                var exception = exceptionFeature.Error;
                _logger.LogError("Exception {Message}", exception.Message);
                BaseResponse<object> response;
                switch (exception)
                {
                    case AppException appException:
                        response = new BaseResponse<object>
                        {
                            Code = appException.ErrorCode.Code,
                            Message = appException.ErrorCode.Message
                        };
                        context.Response.StatusCode = (int)appException.ErrorCode.StatusCode;
                        break;
                    case UnauthorizedAccessException:
                        var unthoreizedErrorCode = ErrorCode.UncategorizedException;
                        response = new BaseResponse<object>
                        {
                            Code = unthoreizedErrorCode.Code,
                            Message = unthoreizedErrorCode.Message
                        };
                        context.Response.StatusCode = (int)unthoreizedErrorCode.StatusCode;
                        break;
                    case ValidationException validationException:
                        var validationErrorCode = ErrorCode.InvalidKey;
                        response = new BaseResponse<object>
                        {
                            Code = validationErrorCode.Code,
                            Message = validationErrorCode.Message
                        };
                        context.Response.StatusCode = (int)validationErrorCode.StatusCode;
                        break;
                    default:
                        response = new BaseResponse<object>
                        {
                            Code = ErrorCode.UncategorizedException.Code,
                            Message = ErrorCode.UncategorizedException.Message
                        };
                        context.Response.StatusCode = (int)ErrorCode.UncategorizedException.StatusCode;
                        break;
                }
                var responseDataJson = System.Text.Json.JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(responseDataJson);
            }
        }
    }
}
