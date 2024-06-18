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
       PersonalInformation dob,
       PersonalInformation gender,
       List<FormQuestion> questions) : base()
    {
        ProgramTitle = programTitle;
        ProgramDescription = programDescription;
        Phone = phone;
        Nationality = nationality;
        Residence = residence;
        IDNumber = idNumber;
        DateOfBirth = dob;
        Gender = gender;
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

