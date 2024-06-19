using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Services.QuestionTypeManagers;

public class YesNoQuestionManager : QuestionTypeManagerBase
{
    public override (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer)
    {
        var baseValidation = base.ValidateAnswer(question, answer);
        if (!baseValidation.IsValid)
            return baseValidation;

        var response = answer.Responses.SingleOrDefault();
        if (response == null)
            return (false, "Answer must be a single response.");

        if (!response.Equals("yes", StringComparison.OrdinalIgnoreCase)
            && !response.Equals("no", StringComparison.OrdinalIgnoreCase))
            return (false, "Invalid response. Must be 'Yes' or 'No'.");

        return (true, string.Empty);
    }
}
