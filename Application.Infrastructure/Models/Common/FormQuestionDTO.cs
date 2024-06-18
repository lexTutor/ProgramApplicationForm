using Application.Core.Entities;
using FluentValidation;
using static Application.Core.Enums;

namespace Application.Infrastructure.Models.Common;

public record FormQuestionDTO(
    int Id,
    string QuestionText,
    int MaximumChoices,
    bool IsOtherOptionEnabled,
    List<string> Options,
    QuestionType QuestionType)
{
    public class FormQuestionDTOValidator : AbstractValidator<FormQuestionDTO>
    {
        public FormQuestionDTOValidator()
        {
            RuleFor(x => x.QuestionType).IsInEnum();
            RuleFor(x => x.QuestionText).NotEmpty();
            RuleFor(x => x.Options).NotEmpty()
               .When(x => x.QuestionType == QuestionType.MultipleChoice
               || x.QuestionType == QuestionType.Dropdown);
            RuleFor(x => x.MaximumChoices).GreaterThan(0)
                .LessThanOrEqualTo(x => x.Options.Count + (x.IsOtherOptionEnabled ? 1 : 0))
                .When(x => x.QuestionType == QuestionType.MultipleChoice);
        }
    }

    public FormQuestion ToEntity(int index)
        => new(index, QuestionText, MaximumChoices, IsOtherOptionEnabled, Options, QuestionType);

    public static FormQuestionDTO ToModel(FormQuestion entity)
    => new(entity.Id, entity.QuestionText, entity.MaximumChoices, entity.IsOtherOptionEnabled,
        entity.Options, entity.QuestionType);
}

