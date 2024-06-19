using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Services.QuestionTypeManagers;

public class ParagraphQuestionManager : QuestionTypeManagerBase
{
    public override (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer)
    {
        var baseValidation = base.ValidateAnswer(question, answer);
        if (!baseValidation.IsValid)
            return baseValidation;

        if (answer.Responses.Count != 1)
            return (false, "Answer must contain a single response.");

        return (true, string.Empty);
    }
}
