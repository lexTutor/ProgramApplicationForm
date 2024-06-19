using Application.Core;
using Application.Core.Entities;
using Application.Infrastructure.Data;
using Application.Infrastructure.Models.Common;
using Application.Infrastructure.Models.Request;
using Application.Infrastructure.Models.Response;
using Application.Infrastructure.Services.QuestionTypeManagers;
using Microsoft.Azure.Cosmos;
using System.Net;
using static Application.Core.Enums;

namespace Application.Infrastructure.Services;

public class ProgramFormService(
    Func<QuestionType, IQuestionTypeManager> questionManagerAccessor,
    ICosmosDbRepository<ProgramForm> programFormRepository,
    ICosmosDbRepository<ProgramFormSubmission> programFormSubmissionRepository) : IProgramFormService
{
    public const string InvalidProgramError = "We couldn't find a program with the provided ID.";
    public const string DuplicateAnswersError = "One or more questions were answered twice";
    public const string QuestionNotAnsweredError = "One or more questions were not answered";

    public async Task<BaseResponse<ProgramFormDataDTO>> CreateProgram(CreateProgramForm request)
    {
        var program = request.ToEntity();

        var validationResults = program.Questions
            .Select(item =>
            {
                var questionManager = questionManagerAccessor(item.QuestionType);
                return questionManager.ValidateQuestion(item);
            });

        if (!validationResults.All(result => result.IsValid))
        {
            var errors = validationResults.Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage))
                .Select(result => result.ErrorMessage).ToArray();

            return BaseResponse<ProgramFormDataDTO>.Fail(Constants.ValidationError, errors);
        }

        await programFormRepository.AddItemAsync(program);

        return BaseResponse<ProgramFormDataDTO>.Success(Constants.Successful, ProgramFormDataDTO.ToModel(program), HttpStatusCode.Created);
    }

    public async Task<BaseResponse<ProgramFormDataDTO>> UpdateProgram(UpdateProgramForm request)
    {
        var program = await programFormRepository.GetItemAsync(request.Id);

        if (program is null)
            return BaseResponse<ProgramFormDataDTO>.Fail(InvalidProgramError);

        program = request.ToEntity(program);

        var validationResults = program.Questions
            .Select(item =>
            {
                var questionManager = questionManagerAccessor(item.QuestionType);
                return questionManager.ValidateQuestion(item);
            });

        if (!validationResults.All(result => result.IsValid))
        {
            var errors = validationResults.Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage))
                .Select(result => result.ErrorMessage).ToArray();

            return BaseResponse<ProgramFormDataDTO>.Fail(Constants.ValidationError, errors);
        }

        await programFormRepository.UpdateItemAsync(program);

        return BaseResponse<ProgramFormDataDTO>.Success(Constants.Successful, ProgramFormDataDTO.ToModel(program));
    }

    public async Task<BaseResponse<ProgramFormDataDTO>> GetProgram(string id)
    {
        var program = await programFormRepository.GetItemAsync(id);

        if (program is null)
            return BaseResponse<ProgramFormDataDTO>.Fail(InvalidProgramError);

        return BaseResponse<ProgramFormDataDTO>.Success(Constants.Successful, ProgramFormDataDTO.ToModel(program));
    }

    public async Task<BaseResponse<List<ProgramFormDataSlimDTO>>> GetAllPrograms()
    {
        var queryString = $"SELECT p.{nameof(ProgramForm.Id).ToLower()}, p.{nameof(ProgramForm.ProgramTitle)}, p.{nameof(ProgramForm.ProgramDescription)} FROM {nameof(ProgramForm)} AS p";

        var programs = await programFormRepository.GetItemsAsync<ProgramFormDataSlimDTO>(new QueryDefinition(queryString));

        return BaseResponse<List<ProgramFormDataSlimDTO>>.Success(Constants.Successful, programs);
    }

    public async Task<BaseResponse<string>> SubmitApplication(CreateProgramFormSubmission request)
    {
        var program = await programFormRepository.GetItemAsync(request.ProgramFormId);

        if (program is null)
            return BaseResponse<string>.Fail(InvalidProgramError);

        var missingFieldErrors = ValidateHiddenFields(program, request);

        if (missingFieldErrors.Length != 0)
            return BaseResponse<string>.Fail(Constants.ValidationError, missingFieldErrors);

        var distinctQuestionIds = request.QuestionAnswers.Select(x => x.QuestionId).Distinct().ToList();

        if (distinctQuestionIds.Count != request.QuestionAnswers.Count)
            return BaseResponse<string>.Fail(DuplicateAnswersError);

        if (distinctQuestionIds.Count != program.Questions.Count)
            return BaseResponse<string>.Fail(QuestionNotAnsweredError);

        var validationResults = request.QuestionAnswers
            .Select(item =>
            {
                var question = program.Questions.Single(x => x.Id == item.QuestionId);
                var questionManager = questionManagerAccessor(question.QuestionType);
                return questionManager.ValidateAnswer(question, item);
            });

        if (!validationResults.All(result => result.IsValid))
        {
            var errors = validationResults.Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage))
                .Select(result => result.ErrorMessage).ToArray();

            return BaseResponse<string>.Fail(Constants.ValidationError, errors);
        }

        var entity = request.ToEntity();
        await programFormSubmissionRepository.AddItemAsync(entity);

        return BaseResponse<string>.Success(Constants.Successful, entity.Id);
    }

    private string[] ValidateHiddenFields(ProgramForm form, CreateProgramFormSubmission request)
    {
        var missingFields = new List<string>();

        if (!form.Phone.IsHiddenFromDisplay && string.IsNullOrWhiteSpace(request.Phone))
            missingFields.Add($"{nameof(form.Phone)} field is required");

        if (!form.Nationality.IsHiddenFromDisplay && string.IsNullOrWhiteSpace(request.Nationality))
            missingFields.Add($"{nameof(form.Nationality)} field is required");

        if (!form.Residence.IsHiddenFromDisplay && string.IsNullOrWhiteSpace(request.Residence))
            missingFields.Add($"{nameof(form.Residence)} field is required");

        if (!form.IDNumber.IsHiddenFromDisplay && string.IsNullOrWhiteSpace(request.IDNumber))
            missingFields.Add($"{nameof(form.IDNumber)} field is required");

        if (!form.DateOfBirth.IsHiddenFromDisplay && request.DateOfBirth is null)
            missingFields.Add($"{nameof(form.DateOfBirth)} field is required");

        if (!form.Gender.IsHiddenFromDisplay && string.IsNullOrWhiteSpace(request.Gender))
            missingFields.Add($"{nameof(form.Gender)} field is required");

        return [.. missingFields];
    }
}
