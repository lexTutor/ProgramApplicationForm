using Application.Infrastructure.Models.Common;
using Application.Infrastructure.Models.Request;
using Application.Infrastructure.Models.Response;

namespace Application.Infrastructure.Services
{
    public interface IProgramFormService
    {
        Task<BaseResponse<ProgramFormDataDTO>> CreateProgram(CreateProgramForm request);
        Task<BaseResponse<List<ProgramFormDataSlimDTO>>> GetAllPrograms();
        Task<BaseResponse<ProgramFormDataDTO>> GetProgram(string id);
        Task<BaseResponse<string>> SubmitApplication(CreateProgramFormSubmission request);
        Task<BaseResponse<ProgramFormDataDTO>> UpdateProgram(UpdateProgramForm request);
    }
}