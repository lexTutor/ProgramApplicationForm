using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using Application.Infrastructure.Services.QuestionTypeManagers;
using Xunit;
using static Application.Core.Enums;

namespace Application.Infrastructure.Test.QuestionTypeManagerTests;

public class DateQuestionManagerTests
{
    private readonly DateQuestionManager _manager;

    public DateQuestionManagerTests()
    {
        _manager = new DateQuestionManager();
    }

    [Fact]
    public void ValidateQuestion_ShouldReturnFalse_WhenQuestionTextIsEmpty()
    {
        var question = new FormQuestion(0, "", 0, false, [], QuestionType.Date);

        var (IsValid, ErrorMessage) = _manager.ValidateQuestion(question);

        Assert.False(IsValid);
        Assert.Equal(QuestionTypeManagerBase.QuestionTextEmptyError, ErrorMessage);
    }

    [Fact]
    public void ValidateQuestion_ShouldReturnTrue_WhenQuestionTextIsValid()
    {
        var question = new FormQuestion(0, "What is the date?", 0, false, [], QuestionType.Date);

        var (IsValid, ErrorMessage) = _manager.ValidateQuestion(question);

        Assert.True(IsValid);
        Assert.Equal(string.Empty, ErrorMessage);
    }

    [Fact]
    public void ValidateAnswer_ShouldReturnFalse_WhenBaseValidationFails_ForInvalidQuestion()
    {
        var answer = new QuestionAnswerDTO(0, []);

        var (IsValid, ErrorMessage) = _manager.ValidateAnswer(null, answer);

        Assert.False(IsValid);
        Assert.Equal(QuestionTypeManagerBase.InvalidQuestionError, ErrorMessage);
    }

    [Fact]
    public void ValidateAnswer_ShouldReturnFalse_WhenBaseValidationFails()
    {
        var question = new FormQuestion(0, "What is the date?", 0, false, [], QuestionType.Date);
        var answer = new QuestionAnswerDTO(0, []);

        var (IsValid, ErrorMessage) = _manager.ValidateAnswer(question, answer);

        Assert.False(IsValid);
        Assert.Equal(QuestionTypeManagerBase.MissingOrInvalidAnswerError, ErrorMessage);
    }


    [Fact]
    public void ValidateAnswer_ShouldReturnFalse_WhenDateFormatIsInvalid()
    {
        var question = new FormQuestion(0, "What is the date?", 0, false, [], QuestionType.Date);
        var answer = new QuestionAnswerDTO(0, ["InvalidDate"]);

        var (IsValid, ErrorMessage) = _manager.ValidateAnswer(question, answer);

        Assert.False(IsValid);
        Assert.Equal(QuestionTypeManagerBase.MissingOrInvalidAnswerError, ErrorMessage);
    }

    [Fact]
    public void ValidateAnswer_ShouldReturnTrue_WhenDateFormatIsValid()
    {
        var question = new FormQuestion(0, "What is the date?", 0, false, [], QuestionType.Date);
        var answer = new QuestionAnswerDTO(0, ["2021-12-31"]);

        var (IsValid, ErrorMessage) = _manager.ValidateAnswer(question, answer);

        Assert.True(IsValid);
        Assert.Equal(string.Empty, ErrorMessage);
    }
}