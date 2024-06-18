using Application.Core.Entities;

namespace Application.Infrastructure.Models.Common;

public record PersonalInformationDTO(
bool IsInternalUseOnly,
bool IsHiddenFromDisplay)
{
    public PersonalInformation ToEntity()
        => new(IsInternalUseOnly, IsHiddenFromDisplay);

    public static PersonalInformationDTO ToModel(PersonalInformation entity)
        => new(entity.IsInternalUseOnly, entity.IsHiddenFromDisplay);
}