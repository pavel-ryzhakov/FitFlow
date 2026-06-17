using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Memberships;

public interface IMembershipService
{
    Task<List<MembershipDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<MembershipDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<MembershipDto>>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<Result<MembershipDto>> CreateAsync(MembershipCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, MembershipUpdateRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}