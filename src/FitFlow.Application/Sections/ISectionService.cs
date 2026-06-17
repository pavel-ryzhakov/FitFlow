namespace FitFlow.Application.Sections;

public interface ISectionService
{
    Task<List<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SectionDto?> CreateAsync(SectionCreationRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Guid id, SectionUpdateRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}