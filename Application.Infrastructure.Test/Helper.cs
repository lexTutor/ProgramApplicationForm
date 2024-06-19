using Application.Core.Entities;
using Application.Infrastructure.Models.Common;
using Application.Infrastructure.Models.Request;
using Application.Infrastructure.Models.Response;
using Application.Infrastructure.Services.QuestionTypeManagers;
using static Application.Core.Enums;

namespace Application.Infrastructure.Test;

public static class Helper
{
    public static Func<QuestionType, IQuestionTypeManager> GetManagerSetup()
    {
        return new Func<QuestionType, IQuestionTypeManager>(type =>
        {
            return type switch
            {
                QuestionType.Paragraph => new ParagraphQuestionManager(),
                QuestionType.YesNo => new YesNoQuestionManager(),
                QuestionType.Dropdown => new DropdownQuestionManager(),
                QuestionType.MultipleChoice => new MultiChoiceQuestionManager(),
                QuestionType.Date => new DateQuestionManager(),
                QuestionType.Number => new NumberQuestionManager(),
                _ => throw new ArgumentException("Invalid QuestionType"),
            };
        });
    }

    public static ProgramForm GetProgramForm()
    {
        var questions = new List<FormQuestion>
        {
            new(0, "Please provide a short bio:", 0, false, [], QuestionType.Paragraph),
            new(1, "Do you have pets?", 0, false, [], QuestionType.YesNo),
            new(2, "What is your favorite color?", 1, true, ["Red", "Blue", "Green"], QuestionType.MultipleChoice),
            new(3, "Which animal do you prefer?", 1, false, ["Cats", "Dogs"], QuestionType.MultipleChoice),
            new(4, "Please select your birth date:", 0, false, [], QuestionType.Date),
            new(5, "How many siblings do you have?", 0, false, [], QuestionType.Number)
        };

        return new("My Program", "This is a description of my program.",
            new PersonalInformation(true, false), new PersonalInformation(true, true), new PersonalInformation(true, true), new PersonalInformation(true, true),
             new PersonalInformation(true, true), new PersonalInformation(true, false), questions);
    }

    public static CreateProgramForm GetCreateProgramForm()
    {
        var questions = new List<FormQuestionDTO>
        {
            new(0, "Please provide a short bio:", 0, false, [], QuestionType.Paragraph),
            new(1, "Do you have pets?", 0, false, [], QuestionType.YesNo),
            new(2, "What is your favorite color?", 1, true, ["Red", "Blue", "Green"], QuestionType.MultipleChoice),
            new(3, "Which animal do you prefer?", 1, false, ["Cats", "Dogs"], QuestionType.MultipleChoice),
            new(4, "Please select your birth date:", 0, false, [], QuestionType.Date),
            new(5, "How many siblings do you have?", 0, false, [], QuestionType.Number)
        };

        return new("My Program", "This is a description of my program.",
            new PersonalInformationDTO(true, false), new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, true),
            new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, false), questions);
    }

    public static UpdateProgramForm GetUpdateProgramForm(string id)
    {
        var questions = new List<FormQuestionDTO>
        {
            new(0, "Please provide a short bio:", 0, false, [], QuestionType.Paragraph),
            new(1, "Do you have pets?", 0, false, [], QuestionType.YesNo),
            new(2, "What is your favorite color?", 1, true, ["Red", "Blue", "Green"], QuestionType.MultipleChoice),
            new(3, "Which animal do you prefer?", 1, false, ["Cats", "Dogs"], QuestionType.MultipleChoice),
            new(4, "Please select your birth date:", 0, false, [], QuestionType.Date),
            new(5, "How many siblings do you have?", 0, false, [], QuestionType.Number)
        };

        return new(id, "My Program", "This is a new description of my program.",
            new PersonalInformationDTO(true, false), new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, true),
            new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, true), new PersonalInformationDTO(true, false), questions);
    }

    public static CreateProgramFormSubmission GetCreateProgramFormSubmission(string id)
    {
        return new CreateProgramFormSubmission(
            ProgramFormId: id,
            FirstName: "John",
            LastName: "Doe",
            Email: "johndoe@example.com",
            Phone: "+1234567890",
            Nationality: null,
            Residence: null,
            IDNumber: null,
            Gender: "Male",
            DateOfBirth: null,
            QuestionAnswers:
            [
                new(0, ["My name is James Doe"]),
                new(1, ["Yes"]),
                new(2, ["Red"]),
                new(3, ["Dogs"]),
                new(4, ["2024-06-18T14:26:22.768Z"]),
                new(5, ["2"])
            ]
        );
    }

    public static ProgramFormDataSlimDTO GetDataSlimDTO()
    {
        return new("7fbf3b86-8762-4c24-9ccc-e49a315c03e5", "My Program", "This is a description of my program.");
    }
}
