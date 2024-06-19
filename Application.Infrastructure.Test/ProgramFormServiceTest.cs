using Application.Core;
using Application.Core.Entities;
using Application.Infrastructure.Data;
using Application.Infrastructure.Models.Common;
using Application.Infrastructure.Models.Request;
using Application.Infrastructure.Models.Response;
using Application.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;
using Xunit;

namespace Application.Infrastructure.Test;

public class ProgramFormServiceTest
{
    private readonly ProgramFormService _programFormService;
    private readonly Mock<ICosmosDbRepository<ProgramFormSubmission>> _mockProgramFormSubmissionRepository;
    private readonly Mock<ICosmosDbRepository<ProgramForm>> _mockProgramFormRepository;

    public ProgramFormServiceTest()
    {
        _mockProgramFormSubmissionRepository = new Mock<ICosmosDbRepository<ProgramFormSubmission>>();

        _mockProgramFormSubmissionRepository.Setup(repo => repo.AddItemAsync(It.IsAny<ProgramFormSubmission>()))
            .Returns(Task.CompletedTask);

        _mockProgramFormRepository = new Mock<ICosmosDbRepository<ProgramForm>>();

        _mockProgramFormRepository.Setup(repo => repo.AddItemAsync(It.IsAny<ProgramForm>()))
            .Returns(Task.CompletedTask);

        _mockProgramFormRepository.Setup(repo => repo.UpdateItemAsync(It.IsAny<ProgramForm>()))
            .Returns(Task.CompletedTask);

        _programFormService = new ProgramFormService(Helper.GetManagerSetup(), _mockProgramFormRepository.Object, _mockProgramFormSubmissionRepository.Object);
    }

    [Theory]
    [InlineData("", "", null, null, null, null, null, null, null)]
    [InlineData("A program", "", null, null, null, null, null, null, null)]
    [InlineData("A program", "A good program", null, null, null, null, null, null, null)]
    public async Task CreateProgramForm_Should_Fail_When_Parameters_Are_Invalid(
        string programTitle, string programDescription, PersonalInformationDTO phone, PersonalInformationDTO nationality, PersonalInformationDTO residence,
        PersonalInformationDTO idNumber, PersonalInformationDTO gender, PersonalInformationDTO dateOfBirth, List<FormQuestionDTO> questions)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _programFormService.CreateProgram(new CreateProgramForm(programTitle, programDescription,
            phone, nationality, residence, idNumber, dateOfBirth, gender, questions)));
    }

    [Fact]
    public async Task CreateProgramForm_Should_Fail_When_Question_Parameter_Is_Invalid()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _programFormService.CreateProgram(
            new CreateProgramForm("A program", "A good program", null, null, null, null, null, null, [])));
    }

    [Fact]
    public async Task CreateProgramForm_Should_Succeed_When_Parameters_Are_Valid()
    {
        //Act
        var result = await _programFormService.CreateProgram(Helper.GetCreateProgramForm());

        //Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Equal(Constants.Successful, result.Message);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProgramForm_Should_Fail_When_Id_Is_Invalid()
    {
        //Arrange
        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(It.IsAny<string>()))
            .ReturnsAsync(value: null);

        //Act
        var updateResult = await _programFormService.UpdateProgram(Helper.GetUpdateProgramForm(Guid.NewGuid().ToString()));

        //Assert
        Assert.False(updateResult.Succeeded);
        Assert.Null(updateResult.Data);
        Assert.Equal(ProgramFormService.InvalidProgramError, updateResult.Message);
        Assert.Equal(HttpStatusCode.BadRequest, updateResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProgramForm_Should_Succeed_When_Parameters_Are_Valid()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        //Act
        var updateResult = await _programFormService.UpdateProgram(Helper.GetUpdateProgramForm(program.Id));

        //Assert
        Assert.True(updateResult.Succeeded);
        Assert.NotNull(updateResult.Data);
        Assert.Equal(program.Id, updateResult.Data.Id);
        Assert.Equal(program.ProgramDescription, updateResult.Data.ProgramDescription);
        Assert.Equal(Constants.Successful, updateResult.Message);
        Assert.Equal(HttpStatusCode.OK, updateResult.StatusCode);
    }

    [Fact]
    public async Task GetProgramForm_Should_Succeed_When_Id_Is_Invalid()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        //Act
        var result = await _programFormService.GetProgram(Guid.NewGuid().ToString());

        //Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal(ProgramFormService.InvalidProgramError, result.Message);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task GetProgramForm_Should_Succeed_When_Parameters_Are_Valid()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        //Act
        var result = await _programFormService.GetProgram(program.Id);

        //Assert
        Assert.True(result.Succeeded);
        Assert.Equal(Constants.Successful, result.Message);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task ListProgramForms_Should_Succeed()
    {
        //Arrange
        _mockProgramFormRepository.Setup(repo => repo.GetItemsAsync<ProgramFormDataSlimDTO>(It.IsAny<QueryDefinition>()))
            .ReturnsAsync([Helper.GetDataSlimDTO()]);

        //Act
        var result = await _programFormService.GetAllPrograms();

        //Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        Assert.Equal(Constants.Successful, result.Message);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task CreateProgramForm_Submission_Should_Fail_When_Id_Is_Invalid()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        //Act
        var submitResult = await _programFormService.SubmitApplication(Helper.GetCreateProgramFormSubmission(Guid.NewGuid().ToString()));

        //Assert
        Assert.False(submitResult.Succeeded);
        Assert.Null(submitResult.Data);
        Assert.Equal(ProgramFormService.InvalidProgramError, submitResult.Message);
        Assert.Equal(HttpStatusCode.BadRequest, submitResult.StatusCode);
    }

    [Fact]
    public async Task CreateProgramForm_Submission_Should_Fail_When_All_Questions_Are_Not_Answered()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        var submission = Helper.GetCreateProgramFormSubmission(program.Id);
        submission.QuestionAnswers.RemoveAt(0);

        //Act
        var submitResult = await _programFormService.SubmitApplication(submission);

        //Assert
        Assert.False(submitResult.Succeeded);
        Assert.Null(submitResult.Data);
        Assert.Equal(ProgramFormService.QuestionNotAnsweredError, submitResult.Message);
        Assert.Equal(HttpStatusCode.BadRequest, submitResult.StatusCode);
    }

    [Fact]
    public async Task CreateProgramForm_Submission_Should_Fail_When_Answers_Are_Duplicated()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        var submission = Helper.GetCreateProgramFormSubmission(program.Id);
        submission.QuestionAnswers.Add(submission.QuestionAnswers[0]);

        //Act
        var submitResult = await _programFormService.SubmitApplication(submission);

        //Assert
        Assert.False(submitResult.Succeeded);
        Assert.Null(submitResult.Data);
        Assert.Equal(ProgramFormService.DuplicateAnswersError, submitResult.Message);
        Assert.Equal(HttpStatusCode.BadRequest, submitResult.StatusCode);
    }

    [Fact]
    public async Task CreateProgramForm_Submission_Should_Succeed_When_Parameters_Are_Valid()
    {
        //Arrange
        var program = Helper.GetProgramForm();

        _mockProgramFormRepository.Setup(repo => repo.GetItemAsync(program.Id))
            .ReturnsAsync(program);

        //Act
        var submitResult = await _programFormService.SubmitApplication(Helper.GetCreateProgramFormSubmission(program.Id));

        //Assert
        Assert.True(submitResult.Succeeded);
        Assert.NotNull(submitResult.Data);
        Assert.Equal(Constants.Successful, submitResult.Message);
        Assert.Equal(HttpStatusCode.OK, submitResult.StatusCode);
    }
}
