﻿using Aplication.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaOnlineApi.Controllers;


namespace WebApi.Middlewares
{

    // ERRORES PARA HTTP
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BaseApiController> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<BaseApiController> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {


            try
            {
               await  _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string> { Accomplished = false, Message = error?.Message };

                switch (error)  
                {
                    case Aplication.Exceptions.AccountException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        //responseModel.Errors = e.Errors;
                        break;
                    case Aplication.Exceptions.ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode=(int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var jsonResponse = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(jsonResponse);


            }          
        }

    }

}
