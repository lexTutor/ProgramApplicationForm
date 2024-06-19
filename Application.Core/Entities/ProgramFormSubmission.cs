namespace Application.Core.Entities;

public class ProgramFormSubmission : BaseEntity
{
    public string ProgramFormId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Nationality { get; set; }
    public string Residence { get; set; }
    public string IDNumber { get; set; }
    public string Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public List<QuestionAnswer> QuestionAnswers { get; set; }

    public ProgramFormSubmission(
        string programFormId,
        string firstName,
        string lastName,
        string email,
        string phone,
        string nationality,
        string residence,
        string iDNumber,
        string gender,
        DateTime? dOB,
        List<QuestionAnswer> questionAnswers)
    {
        if (string.IsNullOrWhiteSpace(programFormId))
            throw new ArgumentNullException(nameof(programFormId), "Application Form ID cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentNullException(nameof(firstName), "Firstname cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentNullException(nameof(lastName), "Lastname cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");

        QuestionAnswers = questionAnswers ?? throw new ArgumentNullException(nameof(questionAnswers), "Question answers list cannot be null.");
        this.ProgramFormId = programFormId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Nationality = nationality;
        Residence = residence;
        IDNumber = iDNumber;
        Gender = gender;
        DateOfBirth = dOB;
    }
}


public class QuestionAnswer(int questionId, List<string> responses)
{
    public int QuestionId { get; set; } = questionId;
    public List<string> Responses { get; set; } = responses;
}