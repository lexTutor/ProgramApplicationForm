using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using FluentValidation;
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
            RuleFor(x => x.Questions).NotEmpty().ChildRules(q => q.RuleFor(c => new FormQuestionDTOValidator()));
        }
    }

    public ProgramForm ToEntity(ProgramForm programForm)
    {
        programForm.ProgramTitle = ProgramTitle;
        programForm.ProgramDescription = ProgramDescription;
        programForm.Phone = Phone.ToEntity();
        programForm.Nationality = Nationality.ToEntity();
        programForm.Residence = Residence.ToEntity();
        programForm.IDNumber = IDNumber.ToEntity();
        programForm.DateOfBirth = DateOfBirth.ToEntity();
        programForm.Gender = Gender.ToEntity();
        programForm.Questions = Questions.Select((x, index) => x.ToEntity(index + 1)).ToList();
        return programForm;
    }
}