using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using FluentValidation;
using static Application.Infrastructure.Models.Common.QuestionAnswerDTO;

namespace Application.Infrastructure.Models.Request;

public record CreateProgramFormSubmission(
    string ApplicationFormId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Nationality,
    string Residence,
    string IDNumber,
    string Gender,
    DateTime? DateOfBirth,
    List<QuestionAnswerDTO> QuestionAnswers)
{
    public ProgramFormSubmission ToEntity()
        => new(ApplicationFormId, FirstName, LastName, Email,
            Phone, Nationality, Residence, IDNumber, Gender, DateOfBirth,
            QuestionAnswers.Select(x => x.ToEntity()).ToList());

    public class ApplicationFormSubmissionDTOValidator : AbstractValidator<CreateProgramFormSubmission>
    {
        public ApplicationFormSubmissionDTOValidator()
        {
            RuleFor(x => x.ApplicationFormId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.QuestionAnswers).NotEmpty().ChildRules(q => q.RuleFor(c => new QuestionAnswerDTOValidator()));
        }
    }
}
