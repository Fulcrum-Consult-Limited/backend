global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Security.Claims;

// Shared
global using Shared.Domain;
global using Shared.Application;

// Domain — enums and interfaces used directly in controllers
global using Identity.Domain.Entities;

// Application layer
global using Identity.Application.Common.Errors;
global using Identity.Application.Users.DTOs;
global using Identity.Application.Users.Queries.GetUserById;
global using Identity.Application.Users.Queries.GetUserByEmail;
global using Identity.Application.Users.Queries.ListUsers;
global using Identity.Application.Users.Commands.DeactivateUser;
global using Identity.Application.Users.Commands.ReactivateUser;
global using Identity.Application.Users.Commands.UpdateUserRole;
global using Identity.Application.Invitations.DTOs;
global using Identity.Application.Invitations.Commands.CreateInvitation;
global using Identity.Application.Invitations.Commands.AcceptInvitation;
global using Identity.Application.Invitations.Commands.ResendInvitation;
global using Identity.Application.Setup.Commands.Bootstrap;
global using Identity.Application.Invitations.Queries.GetInvitationByToken;
global using Identity.Application.Invitations.Queries.ListPendingInvitations;
global using Identity.Application.Auth.DTOs;
global using Identity.Application.Auth.Commands.Login;
global using Identity.Application.Auth.Commands.Logout;
global using Identity.Application.PasswordReset.Commands.RequestPasswordReset;
global using Identity.Application.PasswordReset.Commands.ResetPassword;
global using Identity.Application.PasswordReset.Commands.ChangePassword;

// ASP.NET Core
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;

// API Extensions
global using Identity.API.Extensions;