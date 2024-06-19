using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Services.QuestionTypeManagers;

public interface IQuestionTypeManager
{
    (bool IsValid, string ErrorMessage) ValidateQuestion(FormQuestion question);
    (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer);
}

public abstract class QuestionTypeManagerBase : IQuestionTypeManager
{
    public const string QuestionTextEmptyError = "Question text cannot be empty.";
    public const string MissingOrInvalidAnswerError = "The question is missing or invalid.";
    public const string InvalidQuestionError = "Question is invalid";

    public virtual (bool IsValid, string ErrorMessage) ValidateQuestion(FormQuestion question)
    {
        if (string.IsNullOrWhiteSpace(question.QuestionText))
            return (false, QuestionTextEmptyError);

        return (true, string.Empty);
    }

    public virtual (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer)
    {
        if (question == null)
            return (false, InvalidQuestionError);

        if (answer == null || answer.Responses.Count == 0)
            return (false, MissingOrInvalidAnswerError);

        if (answer.Responses.Any(string.IsNullOrWhiteSpace))
            return (false, MissingOrInvalidAnswerError);

        return (true, string.Empty);
    }
}
