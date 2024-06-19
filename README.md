# ProgramApplicationForm

The ProgramApplicationForm is a C# API designed for creating and managing dynamic program application forms. It offers support for various question types, including paragraphs, yes/no, dropdowns, multiple choice, dates, and numbers. The API also includes functionalities for form submission, validation, and retrieving submitted data.

## Features

- **Dynamic Form Creation**: Create and manage dynamic program application forms tailored to your needs.
- **Question Types**: Support for various question types such as paragraphs, yes/no, dropdowns, multiple choice, dates, and numbers.
- **Validation**: Validate questions and answers to ensure data accuracy.
- **Form Submission**: Submit application forms seamlessly through the API.
- **Data Retrieval**: Retrieve submitted application forms and associated data.

## Technologies Used

The ProgramApplicationForm is built using the following technologies:

- **C#**: The primary programming language for the API.
- **ASP.NET Core**: Framework used for building web APIs.
- **Azure Cosmos DB**: NoSQL database used for storage of form data.
- **Fluent Validation**: Library integrated for robust input validation in the API.
- **XUnit**: Testing framework for unit testing.
- **Moq**: Mocking library used in testing scenarios.

## Usage

### API Endpoints

- **Create Program**: `POST /api/program`

  Sample Payload:
  ```json
  {
      "ProgramTitle": "My Program",
      "ProgramDescription": "This is a description of my program.",
      "FirstName": "John",
      "LastName": "Doe",
      "Email": "johndoe@example.com",
      "Phone": {
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": false
      },
      "Nationality": {
          "Value": null,
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": true
      },
      "Residence": {
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": true
      },
      "IDNumber": {
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": true
      },
      "DateOfBirth": {
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": true
      },
      "Gender": {
          "IsInternalUseOnly": true,
          "IsHiddenFromDisplay": false
      },
      "Questions": [
          {
              "Id": 0,
              "QuestionText": "Please provide a short bio:",
              "MaximumChoices": 0,
              "IsOtherOptionEnabled": false,
              "Options": [],
              "QuestionType": 0
          },
          {
              "Id": 1,
              "QuestionText": "Do you have pets?",
              "MaximumChoices": 0,
              "IsOtherOptionEnabled": false,
              "Options": [],
              "QuestionType": 1
          },
          {
              "Id": 2,
              "QuestionText": "What is your favorite color?",
              "MaximumChoices": 1,
              "IsOtherOptionEnabled": true,
              "Options": ["Red", "Blue", "Green"],
              "QuestionType": 2
          },
          {
              "Id": 3,
              "QuestionText": "Which animal do you prefer?",
              "MaximumChoices": 1,
              "IsOtherOptionEnabled": false,
              "Options": ["Cats", "Dogs"],
              "QuestionType": 3
          },
          {
              "Id": 4,
              "QuestionText": "Please select your birth date:",
              "MaximumChoices": 0,
              "IsOtherOptionEnabled": false,
              "Options": [],
              "QuestionType": 4
          },
          {
              "Id": 5,
              "QuestionText": "How many siblings do you have?",
              "MaximumChoices": 0,
              "IsOtherOptionEnabled": false,
              "Options": [],
              "QuestionType": 5
          }
      ]
  }
  ```

- **Submit Program Application**: `POST /api/program/submit`

  Sample Payload:
  ```json
  {
      "programFormId": "7fbf3b86-8762-4c24-9ccc-e49a315c03e5",
      "firstName": "John",
      "lastName": "Doe",
      "email": "johndoe@example.com",
      "questionAnswers": [
          {
              "questionId": 1,
              "responses": [
                  "Please provide a short bio."
              ]
          },
          {
              "questionId": 2,
              "responses": [
                  "Yes"
              ]
          },
          {
              "questionId": 3,
              "responses": [
                  "Red"
              ]
          },
          {
              "questionId": 4,
              "responses": [
                  "Dogs"
              ]
          },
          {
              "questionId": 5,
              "responses": [
                  "2024-06-18T14:26:22.768Z"
              ]
          },
          {
              "questionId": 6,
              "responses": [
                  "2"
              ]
          }
      ],
      "phone": "+1234567890",
      "nationality": null,
      "residence": null,
      "idNumber": null,
      "dateOfBirth": null,
      "gender": "Male"
  }
  ```

- **Update Program**: `PUT /api/program`

- **Get Program**: `GET /api/program/{formId}`

- **Get All Programs**: `GET /api/program`
