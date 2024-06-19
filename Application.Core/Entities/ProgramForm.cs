using static Application.Core.Enums;

namespace Application.Core.Entities;

public class ProgramForm : BaseEntity
{
    public ProgramForm(
       string programTitle,
       string programDescription,
       PersonalInformation phone,
       PersonalInformation nationality,
       PersonalInformation residence,
       PersonalInformation idNumber,
       PersonalInformation dateOfBirth,
       PersonalInformation gender,
       List<FormQuestion> questions)
    {
        if (string.IsNullOrWhiteSpace(programTitle))
            throw new ArgumentNullException(nameof(programTitle), "Program title cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(programDescription))
            throw new ArgumentNullException(nameof(programDescription), "Program description cannot be null or empty.");

        if (questions == null || questions.Count == 0)
            throw new ArgumentNullException("questions cannot be null or empty.", nameof(questions));
        
        Phone = phone ?? throw new ArgumentNullException(nameof(phone), "Phone cannot be null.");
        Nationality = nationality ?? throw new ArgumentNullException(nameof(nationality), "Nationality cannot be null.");
        Residence = residence ?? throw new ArgumentNullException(nameof(residence), "Residence cannot be null.");
        IDNumber = idNumber ?? throw new ArgumentNullException(nameof(idNumber), "IDNumber cannot be null.");
        DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth), "DateOfBirth cannot be null.");
        Gender = gender ?? throw new ArgumentNullException(nameof(gender), "Gender cannot be null.");
        ProgramTitle = programTitle;
        ProgramDescription = programDescription;
        Questions = questions;
    }

    public string ProgramTitle { get; set; }
    public string ProgramDescription { get; set; }
    public PersonalInformation Phone { get; set; }
    public PersonalInformation Nationality { get; set; }
    public PersonalInformation Residence { get; set; }
    public PersonalInformation IDNumber { get; set; }
    public PersonalInformation DateOfBirth { get; set; }
    public PersonalInformation Gender { get; set; }
    public List<FormQuestion> Questions { get; set; }
}

public class FormQuestion(int id, string questionText, int maximumChoices, bool isOtherOptionEnabled, List<string> options, QuestionType questionType)
{
    public int Id { get; set; } = id;
    public string QuestionText { get; set; } = questionText;
    public int MaximumChoices { get; set; } = maximumChoices;
    public bool IsOtherOptionEnabled { get; set; } = isOtherOptionEnabled;
    public List<string> Options { get; set; } = options;
    public QuestionType QuestionType { get; set; } = questionType;
}

public class PersonalInformation(bool isInternalUseOnly, bool isHiddenFromDisplay)
{
    public bool IsInternalUseOnly { get; set; } = isInternalUseOnly;
    public bool IsHiddenFromDisplay { get; set; } = isHiddenFromDisplay;
}

