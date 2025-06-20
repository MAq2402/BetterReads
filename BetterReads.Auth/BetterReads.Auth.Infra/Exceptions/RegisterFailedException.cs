using System.Net;
using ApplicationException = BetterReads.Shared.Application.Exceptions.ApplicationException;

namespace BetterReads.Auth.Infra.Exceptions;

public class RegisterFailedException(string message) : ApplicationException(message, HttpStatusCode.BadRequest);