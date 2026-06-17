using FitFlow.Application.Clients;
using FitFlow.Application.Common.Results;
using FitFlow.Domain.Entities;
using FitFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitFlow.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly FitFlowDbContext _dbContext;

    public ClientService(FitFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clients
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ClientDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                Email = x.Email,
                BirthDate = x.BirthDate,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<ClientDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Clients
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ClientDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                Email = x.Email,
                BirthDate = x.BirthDate,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (client is null)
        {
            return Result<ClientDto>.Failure(ClientError.NotFound);
        }

        return Result<ClientDto>.Success(client);
    }

    public async Task<Result<ClientDto>> CreateAsync(ClientCreationRequest request, CancellationToken cancellationToken = default)
    {
        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            BirthDate = request.BirthDate,
            IsActive = true
        };

        _dbContext.Clients.Add(client);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var clientDto = new ClientDto
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Phone = client.Phone,
            Email = client.Email,
            BirthDate = client.BirthDate,
            IsActive = client.IsActive
        };

        return Result<ClientDto>.Success(clientDto);
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        ClientUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (client is null)
        {
            return Result.Failure(ClientError.NotFound);
        }

        client.FirstName = request.FirstName;
        client.LastName = request.LastName;
        client.Phone = request.Phone;
        client.Email = request.Email;
        client.BirthDate = request.BirthDate;
        client.IsActive = request.IsActive;
        client.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Clients
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (client is null)
        {
            return Result.Failure(ClientError.NotFound);
        }

        // Мягкое удаление: не удаляем клиента физически, а делаем неактивным.
        // Это правильно для CRM, потому что у клиента могут быть оплаты, посещения и абонементы.
        client.IsActive = false;
        client.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}