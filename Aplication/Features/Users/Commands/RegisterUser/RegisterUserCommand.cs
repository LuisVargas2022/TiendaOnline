﻿using Aplication.DTOs.Account;
using Aplication.Interfaces.Account;
using Aplication.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplications.Feautres.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Response<string>>
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Origin { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<string>>
     {
        private readonly IAccountService _accountService;
        public RegisterUserCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterAsync(new RegisterRequest
            {
                Email=request.Email,
                Password=request.Password,
                ConfirmPassword=request.ConfirmPassword,
                UserName=request.UserName,
                FistName=request.FistName,
                LastName=request.LastName,

            },request.Origin);
        }
    }
}
