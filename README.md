# AssetIQ

### AI-Powered Assistant for Portfolio Management

AssetIQ is an AI-powered portfolio management assistant built with **C#**, **.NET 10**, and **Microsoft Semantic Kernel**.

It allows users to ask natural-language questions about portfolio metrics and uses an LLM to understand the user's intent and orchestrate domain-specific plugins to retrieve the required data and perform calculations.

The project demonstrates how **Generative AI can be combined with existing financial business logic** rather than replacing it.

---

## Overview

Traditional financial applications typically expose portfolio information through predefined screens, reports, and APIs.

AssetIQ explores a more natural interaction model:

> **"What is my net worth?"**

Instead of requiring the user to navigate through multiple screens, AssetIQ:

1. Understands the user's request.
2. Identifies the required financial metric.
3. Retrieves the metric definition and formula.
4. Determines the required portfolio values.
5. Retrieves the relevant client data.
6. Performs the calculation using deterministic C# code.
7. Uses the LLM to explain the result in natural language.

---

## Architecture

```text
                         ┌─────────────────────┐
                         │       User          │
                         │ "What is my        │
                         │    net worth?"      │
                         └──────────┬──────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │    LLM / Semantic   │
                         │       Kernel        │
                         │    Orchestrator     │
                         └──────────┬──────────┘
                                    │
                    ┌───────────────┼────────────────┐
                    │               │                │
                    ▼               ▼                ▼
             ┌────────────┐  ┌────────────┐  ┌──────────────┐
             │  Metrics   │  │ Portfolio  │  │ Calculation  │
             │   Plugin   │  │   Plugin   │  │    Plugin    │
             └────────────┘  └────────────┘  └──────────────┘
                    │               │                │
                    ▼               ▼                ▼
             Find Metric       Get Values        Calculate
                    │               │                │
                    └───────────────┼────────────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │    LLM Response     │
                         │ Natural-language     │
                         │ explanation          │
                         └─────────────────────┘
```

---

## Key Concepts Demonstrated

### 1. Natural Language → Business Capability

The user does not need to know internal metric names.

For example:

```text
"What is my net worth?"
```

The LLM can identify that the user is asking for the `NetWorth` metric.

---

### 2. Metadata-Driven Metrics

Financial metrics are defined separately from the application logic.

Example:

```json
{
  "metric": "NetWorth",
  "displayName": "Net Worth",
  "aliases": [
    "net worth",
    "worth",
    "total worth"
  ],
  "description": "Total net worth available to the client.",
  "formula": "ALB + SPAN + THV",
  "requiredFields": [
    "ALB",
    "SPAN",
    "THV"
  ]
}
```

This allows new metrics to be introduced without hardcoding every metric into the plugin implementation.

---

### 3. Semantic Kernel Plugins

AssetIQ exposes business capabilities to the LLM through Semantic Kernel plugins.

#### MetricsPlugin

Responsible for identifying the financial metric and retrieving its definition.

Example:

```text
FindMetric("What is my net worth?")
```

Returns:

```text
Metric: NetWorth
Formula: ALB + SPAN + THV
Required Fields: ALB, SPAN, THV
```

---

#### PortfolioPlugin

Responsible for retrieving portfolio values for the current client.

Example:

```text
GetPortfolioValues(
    ALB,
    SPAN,
    THV
)
```

The plugin retrieves only the required fields rather than exposing the entire portfolio to the LLM.

---

#### CalculationPlugin

Responsible for performing deterministic financial calculations.

For example:

```text
ALB + SPAN + THV
```

with:

```text
ALB  = 200000
SPAN = 80
THV  = 40
```

produces:

```text
200120
```

The calculation is performed by C# rather than relying on the LLM for financial arithmetic.

---

## Why Use This Architecture?

AssetIQ intentionally separates **AI reasoning** from **financial business logic**.

### LLM

The LLM is responsible for:

- Understanding natural-language requests
- Identifying the user's intent
- Selecting appropriate tools/plugins
- Orchestrating multiple plugin calls
- Handling conversational follow-up questions
- Explaining results to the user

### C# Application

The application remains responsible for:

- Reading metric definitions
- Retrieving portfolio data
- Applying business rules
- Performing calculations
- Validating data
- Maintaining deterministic behavior

This separation makes the system easier to test, maintain, and extend.

---

## Example Conversation

### User

```text
What is my net worth?
```

### AssetIQ

The LLM identifies the `NetWorth` metric.

```text
Formula:
ALB + SPAN + THV

Required Fields:
ALB
SPAN
THV
```

The Portfolio Plugin retrieves the values for the current client.

```text
ALB  = 200000
SPAN = 80
THV  = 40
```

The Calculation Plugin calculates:

```text
200000 + 80 + 40 = 200120
```

### Response

```text
Your total net worth is 200,120.

It is calculated as:

ALB + SPAN + THV

200,000 + 80 + 40 = 200,120
```

---

## Conversational Context

AssetIQ uses Semantic Kernel's `ChatHistory` to maintain conversation context.

This allows follow-up questions such as:

```text
User:
What is my net worth?

Assistant:
Your net worth is 200,120.

User:
Why?

Assistant:
Your net worth is calculated using ALB + SPAN + THV...
```

The second question does not need to repeat the original context.

---

## Token Efficiency

A key design consideration is limiting the amount of structured data sent to the LLM.

Instead of exposing every available portfolio metric:

```text
ALB
SPAN
THV
PLEDGE
MTF
...
```

the system first identifies the required metric and its required fields.

For `NetWorth`:

```text
RequiredFields:
ALB
SPAN
THV
```

The Portfolio Plugin returns only those values.

This reduces unnecessary LLM input and keeps domain-specific data retrieval within the application.

---

## Technology Stack

- **C#**
- **.NET 10**
- **Windows Forms**
- **Microsoft Semantic Kernel**
- **OpenAI-compatible LLM**
- **System.Text.Json**
- **Semantic Kernel Function Calling**
- **ChatHistory**
- **JSON-based metric metadata**

---

## Project Structure

```text
AssetIQ
│
├── Data
│   ├── metrics.json
│   └── portfolio.json
│
├── Models
│   ├── MetricDefinition.cs
│   ├── ClientPortfolio.cs
│   └── CalculationResult.cs
│
├── MetricsPlugin.cs
├── PortfolioPlugin.cs
├── CalculationPlugin.cs
│
├── MainForm.cs
└── Program.cs
```

---

## Getting Started

### Prerequisites

- Visual Studio 2022+
- .NET 10 SDK
- OpenAI-compatible API access
- Semantic Kernel NuGet packages

### Configuration

Configure your LLM credentials using your preferred secure configuration mechanism.

**Do not commit API keys or secrets to GitHub.**

For local development, use:

- User Secrets
- Environment variables
- Secure configuration providers

---

## Running the Application

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Configure your LLM credentials.
4. Build the solution.
5. Run the WinForms application.
6. Enter a natural-language portfolio question.

Example:

```text
What is my net worth?
```

---

## Current Capabilities

- [x] Natural-language portfolio questions
- [x] Semantic Kernel function calling
- [x] Metadata-driven financial metrics
- [x] Metric aliases
- [x] Formula-based metrics
- [x] Client-specific portfolio data
- [x] Multiple plugin orchestration
- [x] Deterministic calculations
- [x] Conversational context using ChatHistory
- [x] WinForms user interface

---

## Future Enhancements

Potential future improvements include:

- SQL Server integration instead of JSON portfolio data
- Real-time portfolio data APIs
- More financial metrics
- Formula validation
- Support for complex expressions and parentheses
- Portfolio performance analysis
- Historical comparisons
- "Why did my net worth change?" analysis
- Risk and exposure analysis
- Margin utilization analysis
- Audit logging of plugin calls
- Authentication and authorization
- Role-based access to portfolio information
- Structured tool execution tracing
- Guardrails for financial calculations and sensitive operations

---

## Design Philosophy

AssetIQ follows a simple principle:

> **Let the LLM reason about the user's intent. Let deterministic application code handle financial data and calculations.**

The LLM acts as an orchestrator over well-defined business capabilities rather than replacing the underlying financial domain logic.

This approach allows existing enterprise systems and business rules to be exposed through a natural-language interface while retaining control, testability, and predictability.

---

## Disclaimer

AssetIQ is an educational and experimental project demonstrating AI orchestration and portfolio-management concepts.

The sample portfolio data is fictional and should not be used for actual investment, trading, or financial decision-making.

---

## Author

Built as a hands-on exploration of:

**C# + .NET + Capital Markets + Generative AI + Semantic Kernel**

The project focuses on applying AI concepts to a realistic financial-domain use case rather than building a generic chatbot.
