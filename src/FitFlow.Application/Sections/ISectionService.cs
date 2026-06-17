using FitFlow.Application.Common.Results;

namespace FitFlow.Application.Sections;

public interface ISectionService
{
    Task<List<SectionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<SectionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<SectionDto>> CreateAsync(SectionCreationRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, SectionUpdateRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}