using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using FluentValidation;
using System.Reflection;
using System.Security.Policy;
using static Application.Infrastructure.Models.Common.FormQuestionDTO;

namespace Application.Infrastructure.Models.Request;

public record UpdateProgramForm(
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
    public class UpdateProgramFormValidator : AbstractValidator<UpdateProgramForm>
    {
        public UpdateProgramFormValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ProgramTitle).NotEmpty();
            RuleFor(x => x.ProgramDescription).NotEmpty();
            RuleFor(x => x.Phone).NotNull();
            RuleFor(x => x.Nationality).NotNull();
            RuleFor(x => x.Residence).NotNull();
            RuleFor(x => x.IDNumber).NotNull();
            RuleFor(x => x.DateOfBirth).NotNull();
            RuleFor(x => x.Gender).NotNull();
            RuleFor(x => x.Questions).NotEmpty();
            RuleForEach(x => x.Questions).SetValidator(new FormQuestionDTOValidator());
        }
    }

    public ProgramForm ToEntity(ProgramForm programForm)
    {
        programForm.ProgramTitle = ProgramTitle;
        programForm.ProgramDescription = ProgramDescription;
        programForm.Phone = Phone?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.Phone), "Phone cannot be null.");
        programForm.Nationality = Nationality?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.Nationality), "Nationality cannot be null.");
        programForm.Residence = Residence?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.Residence), "Residence cannot be null.");
        programForm.IDNumber = IDNumber?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.IDNumber), "IDNumber cannot be null.");
        programForm.DateOfBirth = DateOfBirth?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.DateOfBirth), "DateOfBirth cannot be null.");
        programForm.Gender = Gender?.ToEntity() ?? throw new ArgumentNullException(nameof(programForm.Gender), "Gender cannot be null.");
        programForm.Questions = Questions.Select((x, index) => x.ToEntity(index + 1)).ToList();
        return programForm;
    }
}