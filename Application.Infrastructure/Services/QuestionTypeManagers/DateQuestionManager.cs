using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Services.QuestionTypeManagers;

public class DateQuestionManager : QuestionTypeManagerBase
{
    public override (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer)
    {
        var baseValidation = base.ValidateAnswer(question, answer);
        if (!baseValidation.IsValid)
            return baseValidation;

        var response = answer.Responses.Single();

        if (!DateTime.TryParse(response, out _))
            return (false, MissingOrInvalidAnswerError);

        return (true, string.Empty);
    }
}
