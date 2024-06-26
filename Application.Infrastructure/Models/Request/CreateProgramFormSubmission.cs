﻿using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using FluentValidation;
using static Application.Infrastructure.Models.Common.QuestionAnswerDTO;

namespace Application.Infrastructure.Models.Request;

public record CreateProgramFormSubmission(
    string ProgramFormId,
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
        => new(ProgramFormId, FirstName, LastName, Email,
            Phone, Nationality, Residence, IDNumber, Gender, DateOfBirth,
            QuestionAnswers?.Select(x => x?.ToEntity()).ToList());

    public class ApplicationFormSubmissionDTOValidator : AbstractValidator<CreateProgramFormSubmission>
    {
        public ApplicationFormSubmissionDTOValidator()
        {
            RuleFor(x => x.ProgramFormId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.QuestionAnswers).NotEmpty();
            RuleForEach(x => x.QuestionAnswers).SetValidator(new QuestionAnswerDTOValidator());
        }
    }
}
