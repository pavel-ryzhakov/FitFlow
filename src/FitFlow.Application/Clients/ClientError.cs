using FitFlow.Application.Common.Errors;

namespace FitFlow.Application.Clients;

public static class ClientError
{
    public static readonly Error NotFound = new(
        "Clients.NotFound",
        "Client was not found.");

    public static readonly Error Inactive = new(
        "Clients.Inactive",
        "Client is inactive.");
}