# ProgramApplicationForm

A C# API for creating and managing dynamic program application forms. The API supports various question types, including paragraphs, yes/no, dropdowns, multiple choice, dates, and numbers. It also includes functionality for form submission, validation, and retrieving submitted data.

## Features

- Create and manage dynamic program application forms
- Support for multiple question types
- Validate questions and answers
- Submit application forms
- Retrieve submitted application forms

## Technologies Used

- C#
- ASP.NET Core
- Azure Cosmos DB (for NoSQL storage)
- XUnit (for unit testing)
- Moq (for mocking in tests)

## Usage

### API Endpoints

- **Create Program**: `POST /api/program`
- **Update Program**: `PUT /api/program`
- **Submit Program Application**: `POST /api/program//submit`
- **Get Program**: `GET /api/program/{formId}`
- **Get All Programs**: `GET /api/program`
