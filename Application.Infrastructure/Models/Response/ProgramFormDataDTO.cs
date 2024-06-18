using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Models.Response;

public record ProgramFormDataDTO(
    string Id,
    string ProgramTitle,
    string ProgramDescription,
    PersonalInformationDTO Phone,
    PersonalInformationDTO Nationality,
    PersonalInformationDTO Residence,
    PersonalInformationDTO IDNumber,
    PersonalInformationDTO DateOfBirth,
    PersonalInformationDTO Gender,
    List<FormQuestionDTO> Questions)
{
    public static ProgramFormDataDTO ToModel(ProgramForm entity)
    {
        return new ProgramFormDataDTO(
            entity.Id,
            entity.ProgramTitle,
            entity.ProgramDescription,
            PersonalInformationDTO.ToModel(entity.Phone),
            PersonalInformationDTO.ToModel(entity.Nationality),
            PersonalInformationDTO.ToModel(entity.Residence),
            PersonalInformationDTO.ToModel(entity.IDNumber),
            PersonalInformationDTO.ToModel(entity.DateOfBirth),
            PersonalInformationDTO.ToModel(entity.Gender),
            entity.Questions.Select(FormQuestionDTO.ToModel).ToList());
    }
}

public record ProgramFormDataSlimDTO(string Id, string ProgramTitle, string ProgramDescription)
{
    public static ProgramFormDataSlimDTO ToModel(ProgramForm entity)

      => new(entity.Id, entity.ProgramTitle, entity.ProgramDescription);
}