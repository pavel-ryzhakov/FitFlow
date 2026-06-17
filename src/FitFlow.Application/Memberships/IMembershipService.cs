namespace FitFlow.Application.Memberships;

public interface IMembershipService
{
    Task<List<MembershipDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<MembershipDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<MembershipDto>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);

    Task<MembershipDto?> CreateAsync(MembershipCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid id, MembershipUpdateRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}