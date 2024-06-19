using Application.Core.Entities;
using Application.Infrastructure.Models.Common;

namespace Application.Infrastructure.Services.QuestionTypeManagers;

public class MultiChoiceQuestionManager : QuestionTypeManagerBase
{
    public override (bool IsValid, string ErrorMessage) ValidateAnswer(FormQuestion question, QuestionAnswerDTO answer)
    {
        var baseValidation = base.ValidateAnswer(question, answer);
        if (!baseValidation.IsValid)
            return baseValidation;

        if (answer.Responses.Count > question.MaximumChoices)
            return (false, $"Answer exceeds the maximum number of choices ({question.MaximumChoices}).");

        int maxDifference = question.IsOtherOptionEnabled ? 1 : 0;

        if (answer.Responses.Except(question.Options).Count() > maxDifference)
            return (false, "Invalid response for Multichoice question.");

        return (true, string.Empty);
    }

    public override (bool IsValid, string ErrorMessage) ValidateQuestion(FormQuestion question)
    {
        var baseValidation = base.ValidateQuestion(question);
        if (!baseValidation.IsValid)
            return baseValidation;

        if (question.Options == null || question.Options.Count == 0)
            return (false, "Multichoice Question choice options cannot be empty.");

        if (question.Options.Any(string.IsNullOrWhiteSpace))
            return (false, "Question choice option cannot be an empty value.");

        if (question.IsOtherOptionEnabled)
            question.Options.Add("Other");

        return (true, string.Empty);
    }
}

