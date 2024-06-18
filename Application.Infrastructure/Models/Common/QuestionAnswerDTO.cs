using Application.Core.Entities;
using FluentValidation;

namespace Application.Infrastructure.Models.Common;
public record QuestionAnswerDTO(int QuestionId, List<string> Responses)
{
    public QuestionAnswer ToEntity() => new(QuestionId, Responses);

    public static QuestionAnswerDTO ToModel(QuestionAnswer entity) => new(entity.QuestionId, entity.Responses);

    public class QuestionAnswerDTOValidator : AbstractValidator<QuestionAnswerDTO>
    {
        public QuestionAnswerDTOValidator()
        {
            RuleFor(x => x.QuestionId).GreaterThan(0);
            RuleFor(x => x.Responses).NotEmpty().ForEach(x => x.NotEmpty());
        }
    }
}
