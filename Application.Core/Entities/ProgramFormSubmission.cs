namespace Application.Core.Entities;

public class ProgramFormSubmission(string applicationFormId, string firstName, string lastName, string email,
    string phone, string nationality, string residence,
    string iDNumber, string gender, DateTime? dOB, List<QuestionAnswer> questionAnswers) : BaseEntity
{
    public string ProgramFormId { get; set; } = applicationFormId;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Phone { get; set; } = phone;
    public string Nationality { get; set; } = nationality;
    public string Residence { get; set; } = residence;
    public string IDNumber { get; set; } = iDNumber;
    public string Gender { get; set; } = gender;
    public DateTime? DateOfBirth { get; set; } = dOB;
    public List<QuestionAnswer> QuestionAnswers { get; set; } = questionAnswers;
}

public class QuestionAnswer(int questionId, List<string> responses)
{
    public int QuestionId { get; set; } = questionId;
    public List<string> Responses { get; set; } = responses;
}