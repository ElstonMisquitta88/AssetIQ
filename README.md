# AssetIQ

### AI-Powered Assistant for Portfolio Management

AssetIQ is an AI-powered portfolio management assistant built using **C#**, **.NET**, **Microsoft Semantic Kernel**, and the **Model Context Protocol (MCP)**.

It allows users to ask natural-language questions about portfolio metrics and uses an LLM to understand the user's intent, orchestrate business capabilities, retrieve portfolio data, and provide natural-language explanations.

The project demonstrates how **Generative AI can be added on top of existing enterprise and financial business logic rather than replacing deterministic application code**.

---


<img width="1706" height="536" alt="Img_01" src="https://github.com/user-attachments/assets/6e579131-25c1-4d9b-a215-51555a36b8aa" />


<img width="1700" height="532" alt="Img_02" src="https://github.com/user-attachments/assets/df199d26-9a76-48d9-b33c-0369b3512ee6" />

# Overview

Traditional portfolio management applications typically expose information through predefined screens, reports, APIs, and database queries.

AssetIQ explores a more natural interaction model.

For example:

> **"What is my net worth?"**

Instead of requiring the user to know:

- Which database table contains the information
- Which fields are required
- How the metric is calculated
- Which API or service should be called

AssetIQ allows the LLM to orchestrate these capabilities.

The application performs the following workflow:

1. Understand the user's question.
2. Identify the financial metric being requested.
3. Retrieve the metric definition.
4. Identify the required portfolio fields.
5. Retrieve the required client portfolio values.
6. Perform the financial calculation using deterministic C# logic.
7. Explain the result in natural language.

---

# Initial Architecture – Semantic Kernel Plugins

The initial version of AssetIQ used **Semantic Kernel plugins** for all business capabilities.

```text
                         ┌─────────────────────┐
                         │       User          │
                         │ "What is my         │
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
             └─────┬──────┘  └─────┬──────┘  └──────┬───────┘
                   │               │                │
                   ▼               ▼                ▼
              Find Metric      Get Values       Calculate
                   │               │                │
                   └───────────────┼────────────────┘
                                   │
                                   ▼
                         ┌─────────────────────┐
                         │    LLM Response     │
                         │ Natural-language    │
                         │ explanation         │
                         └─────────────────────┘
```

In this architecture, all business capabilities were implemented as **in-process Semantic Kernel plugins**.

The LLM was responsible for deciding which plugin to call and in what sequence.

---

# Semantic Kernel Plugins

## MetricsPlugin

The `MetricsPlugin` is responsible for identifying the financial metric requested by the user.

Example:

```text
What is my net worth?
```

The plugin identifies:

```text
Metric: NetWorth

Formula:

ALB + SPAN + THV

Required Fields:

ALB
SPAN
THV
```

The metric definition is stored as metadata rather than being hardcoded into the LLM prompt.

---

## PortfolioPlugin

The `PortfolioPlugin` retrieves portfolio values for the current client.

For example:

```text
Required Fields:

ALB
SPAN
THV
```

The plugin retrieves only the required portfolio values.

Example:

```text
ALB  = 100
SPAN = 100
THV  = 100
```

The current client is determined by the application.

The LLM does not need to request or manage the client identifier.

---

## CalculationPlugin

The original `CalculationPlugin` performed deterministic financial calculations inside the AssetIQ application.

Example:

```text
Formula:

ALB + SPAN + THV
```

Inputs:

```text
ALB  = 100
SPAN = 100
THV  = 100
```

Result:

```text
300
```

The calculation logic was deterministic C# code.

The LLM was responsible for orchestrating the calculation, but not for performing the financial arithmetic.

---

# Metadata-Driven Financial Metrics

AssetIQ uses metadata-driven financial metric definitions.

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

New metrics can therefore be added without modifying the LLM orchestration logic.

---

# LLM Tool Orchestration

The LLM does not directly access portfolio data or perform calculations.

Instead, it orchestrates the available business capabilities.

```text
User Question

"What is my net worth?"
        │
        ▼
MetricsPlugin
        │
        ▼
NetWorth
Formula:
ALB + SPAN + THV
        │
        ▼
PortfolioPlugin
        │
        ▼
ALB = 100
SPAN = 100
THV = 100
        │
        ▼
CalculationPlugin
        │
        ▼
Result = 300
        │
        ▼
LLM Explanation
```

This demonstrates **LLM-based tool orchestration**.

---

# System Prompt

AssetIQ uses a system message to guide the LLM's behavior.

Conceptually:

```text
You are AssetIQ, a portfolio management assistant.

When answering a portfolio question:

1. Identify the financial metric requested by the user.
2. Use MetricsPlugin to retrieve the metric definition.
3. Use the required fields from the metric definition.
4. Retrieve portfolio values using PortfolioPlugin.
5. Use the calculation capability to perform the calculation.
6. Explain the result clearly.

Never invent portfolio values.
Never assume missing financial data.
Do not ask the user for portfolio values when they can be
retrieved using available tools.

The current client is determined by the application.
```

---

# Conversational Context

AssetIQ uses Semantic Kernel's `ChatHistory` to maintain conversational context.

This allows users to ask follow-up questions.

Example:

```text
User:

What is my net worth?

Assistant:

Your net worth is 300.

User:

Why?

Assistant:

Your net worth is calculated using ALB + SPAN + THV.
```

---

# Token Efficiency

Instead of sending an entire client portfolio to the LLM:

```text
ALB
SPAN
THV
PLEDGE
MTF
...
```

AssetIQ first identifies the required metric and fields.

For `NetWorth`:

```text
Required Fields:

ALB
SPAN
THV
```

The Portfolio Plugin then retrieves only those values.

---

# Enhancement – Introducing Model Context Protocol (MCP)

After implementing the initial Semantic Kernel plugin architecture, the project was enhanced to explore the **Model Context Protocol (MCP)**.

The calculation capability was selected as the first business capability to be exposed through MCP.

The goal was to understand how an existing C# business capability could move from:

```text
In-Process Semantic Kernel Plugin
```

to:

```text
Reusable MCP Tool
```

without rewriting the underlying calculation logic.

---

# Original Calculation Flow

```text
LLM
 │
 ▼
Semantic Kernel
 │
 ▼
CalculationPlugin
 │
 ▼
Deterministic C# Calculation
 │
 ▼
Result
```

The calculation logic originally existed directly within the AssetIQ application.

---

# MCP Enhancement

The calculation capability was then exposed through a separate MCP server.

```text
AssetIQ Application
        │
        ▼
     MCP Client
        │
        │ STDIO
        ▼
Calculation MCP Server
        │
        ▼
    calculate Tool
        │
        ▼
Deterministic C# Logic
        │
        ▼
       Result
```

The underlying business calculation remained deterministic C# code.

Only the way the capability was exposed changed.

Before:

```text
C# Calculation Method
        │
        ▼
Semantic Kernel Plugin
```

After:

```text
C# Calculation Method
        │
        ▼
       MCP Tool
        │
        ▼
     MCP Server
```

---

# Model Context Protocol (MCP)

The Model Context Protocol provides a standard way for applications to expose and consume AI tools.

In AssetIQ:

```text
AssetIQ Application
        │
        ▼
     MCP Client
        │
        ▼
Calculation MCP Server
        │
        ▼
   calculate Tool
```

The MCP server exposes a calculation tool named:

```text
calculate
```

Example input:

```json
{
  "formula": "ALB + SPAN + THV",
  "inputs": {
    "ALB": 100,
    "SPAN": 100,
    "THV": 100
  }
}
```

Example result:

```text
300
```

---

# Why Use MCP?

Semantic Kernel plugins are registered directly inside an application.

MCP provides a standard way to expose capabilities outside the application process.

This means a capability can potentially be reused by multiple AI applications.

```text
                     ┌──────────────────┐
                     │ Calculation MCP  │
                     │      Server      │
                     └────────┬─────────┘
                              │
                         MCP Protocol
                              │
             ┌────────────────┼────────────────┐
             ▼                ▼                ▼
          AssetIQ        AI Application    Other Client
```

---

# Current Architecture – Semantic Kernel + MCP

The current version of AssetIQ uses a hybrid architecture.

Some capabilities remain as in-process Semantic Kernel plugins, while the calculation capability is provided through an MCP server.

```text
                         ┌─────────────────────┐
                         │       User          │
                         │ "What is my         │
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
                    ┌───────────────┼──────────────────┐
                    │               │                  │
                    ▼               ▼                  ▼
             ┌────────────┐  ┌────────────┐  ┌──────────────────┐
             │  Metrics   │  │ Portfolio  │  │   Calculation    │
             │   Plugin   │  │   Plugin   │  │     Adapter      │
             └─────┬──────┘  └─────┬──────┘  └────────┬─────────┘
                   │               │                   │
                   ▼               ▼                   ▼
              Find Metric      Get Values           MCP Client
                                                       │
                                                       │ STDIO
                                                       ▼
                                             ┌───────────────────┐
                                             │ Calculation MCP   │
                                             │      Server       │
                                             └─────────┬─────────┘
                                                       │
                                                       ▼
                                                  calculate
                                                       │
                                                       ▼
                                             Deterministic C#
                                             Calculation Logic
                                                       │
                                                       ▼
                                              Calculation Result
                                                       │
                                                       ▼
                                             ┌───────────────────┐
                                             │   LLM Response    │
                                             │ Natural-language  │
                                             │   explanation     │
                                             └───────────────────┘
```

The LLM continues to orchestrate the complete workflow, while the calculation capability is accessed through MCP.

---

# Semantic Kernel + MCP Integration

```text
Semantic Kernel
      │
      ├── MetricsPlugin
      │
      ├── PortfolioPlugin
      │
      └── Calculation Adapter
                │
                ▼
             MCP Client
                │
                ▼
        Calculation MCP Server
```

> **Semantic Kernel orchestrates capabilities, while MCP provides a standard way to expose capabilities outside the application.**

---

# MCP Tool Discovery

AssetIQ connects to the MCP server and discovers the available tools.

```text
MCP Client
    │
    ▼
ListToolsAsync()
    │
    ▼
calculate
```

The discovered MCP tool can then be integrated into the Semantic Kernel workflow.

```text
MCP Tool
    │
    ▼
AIFunction
    │
    ▼
AsKernelFunction()
    │
    ▼
Semantic Kernel
```

---

# Complex Input Serialization

During the Semantic Kernel and MCP integration, an interoperability issue was encountered involving structured parameters.

The calculation tool accepts:

```csharp
Dictionary<string, decimal> inputs
```

Direct MCP invocation worked correctly.

However, when the MCP function was invoked through Semantic Kernel, the nested dictionary needed to be represented as JSON-compatible data.

The solution was to use a calculation adapter.

```text
LLM
 │
 ▼
Semantic Kernel
 │
 ▼
Calculation Adapter
 │
 │ Dictionary<string, decimal>
 ▼
JsonSerializer.SerializeToElement()
 │
 ▼
JsonElement
 │
 ▼
MCP Tool
 │
 ▼
Calculation MCP Server
```

The adapter keeps the LLM-facing tool interface simple while handling serialization requirements internally.

---

# End-to-End Current Flow

For the question:

```text
What is my net worth?
```

The current AssetIQ workflow is:

```text
User Question
      │
      ▼
Semantic Kernel + LLM
      │
      ▼
MetricsPlugin
      │
      │ Finds:
      │ NetWorth
      │ Formula: ALB + SPAN + THV
      ▼
PortfolioPlugin
      │
      │ Retrieves:
      │ ALB = 100
      │ SPAN = 100
      │ THV = 100
      ▼
Calculation Adapter
      │
      ▼
MCP Client
      │
      ▼
Calculation MCP Server
      │
      ▼
calculate
      │
      ▼
Result = 300
      │
      ▼
LLM Response
```

The LLM orchestrates the workflow, while the financial calculation remains deterministic.

---

# Architecture Evolution

## Stage 1 – Semantic Kernel Plugins

```text
User
  │
  ▼
LLM + Semantic Kernel
  │
  ├── MetricsPlugin
  ├── PortfolioPlugin
  └── CalculationPlugin
```

All capabilities existed inside the application.

## Stage 2 – Calculation Capability Extracted

```text
Calculation Logic
        │
        ▼
     MCP Server
        │
        ▼
   calculate Tool
```

## Stage 3 – Current Hybrid Architecture

```text
User
  │
  ▼
LLM + Semantic Kernel
  │
  ├── MetricsPlugin
  │
  ├── PortfolioPlugin
  │
  └── Calculation Adapter
          │
          ▼
       MCP Client
          │
          ▼
Calculation MCP Server
          │
          ▼
     calculate Tool
          │
          ▼
Deterministic C# Logic
```

This demonstrates the progression from:

```text
Traditional C# Business Logic
            │
            ▼
Semantic Kernel Plugins
            │
            ▼
LLM Function Calling
            │
            ▼
Tool Orchestration
            │
            ▼
Model Context Protocol
            │
            ▼
Reusable AI Business Capabilities
```

---

# Example Conversation

## User

```text
What is my net worth?
```

## MetricsPlugin

```text
Metric: NetWorth

Formula:

ALB + SPAN + THV

Required Fields:

ALB
SPAN
THV
```

## PortfolioPlugin

```text
ALB  = 100
SPAN = 100
THV  = 100
```

## Calculation MCP Tool

```json
{
  "formula": "ALB + SPAN + THV",
  "inputs": {
    "ALB": 100,
    "SPAN": 100,
    "THV": 100
  }
}
```

Result:

```text
300
```

## Final Response

```text
Your net worth, which represents the total net worth available to you,
is calculated as the sum of ALB, SPAN, and THV values in your portfolio.

Based on your portfolio data, your net worth is 300.
```

---

# Why This Architecture?

AssetIQ intentionally separates AI reasoning from deterministic financial logic.

## LLM Responsibilities

- Understanding natural-language questions
- Identifying user intent
- Selecting available tools
- Orchestrating tool calls
- Maintaining conversational context
- Explaining results

## Application Responsibilities

- Retrieving metric definitions
- Retrieving client portfolio data
- Applying business rules
- Managing client context
- Validating inputs
- Performing deterministic operations

## MCP Server Responsibilities

- Exposing reusable business capabilities
- Receiving structured tool arguments
- Executing deterministic calculations
- Returning calculation results

This separation improves:

- Testability
- Predictability
- Maintainability
- Reusability
- Separation of concerns

---

# Technology Stack

- **C#**
- **.NET 10**
- **Windows Forms**
- **Microsoft Semantic Kernel**
- **Semantic Kernel Function Calling**
- **Model Context Protocol (MCP)**
- **MCP Client / Server Architecture**
- **Microsoft.Extensions.AI**
- **OpenAI-compatible LLM**
- **System.Text.Json**
- **ChatHistory**
- **JSON-based metric metadata**

---

# Current Capabilities

- [x] Natural-language portfolio questions
- [x] Semantic Kernel function calling
- [x] Metadata-driven financial metrics
- [x] Metric aliases
- [x] Formula-based metrics
- [x] Client-specific portfolio data
- [x] Multiple tool orchestration
- [x] Deterministic financial calculations
- [x] Conversational context using ChatHistory
- [x] WinForms user interface
- [x] MCP server for calculation capability
- [x] MCP client integration
- [x] MCP tool discovery
- [x] Semantic Kernel and MCP integration
- [x] Calculation adapter for structured MCP input
- [x] Hybrid in-process plugin and external MCP architecture

---

# Future Enhancements

- SQL Server integration instead of JSON portfolio data
- Real-time portfolio data APIs
- Additional financial metrics
- Formula validation
- Support for complex formulas and parentheses
- Portfolio performance analysis
- Historical portfolio comparison
- "Why did my net worth change?" analysis
- Risk and exposure analysis
- Margin utilization analysis
- Additional MCP servers
- MCP-based Portfolio capabilities
- MCP-based Risk calculation capabilities
- Audit logging of tool calls
- Authentication and authorization
- Role-based access
- Structured tool execution tracing
- AI guardrails
- Multi-agent workflows for more complex analysis

---

# Getting Started

## Prerequisites

- Visual Studio 2022 or later
- .NET 10 SDK
- Access to an OpenAI-compatible LLM
- Required Semantic Kernel packages
- Required MCP SDK packages

## Setup

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Configure your LLM credentials using a secure configuration mechanism.
4. Build the AssetIQ application.
5. Build the Calculation MCP Server.
6. Run the AssetIQ application.
7. Ask a portfolio-related question.

Example:

```text
What is my net worth?
```

---

# Security

API keys and secrets should not be committed to the repository.

For local development, use secure configuration mechanisms such as:

- User Secrets
- Environment variables
- Secure configuration providers

---

# Design Philosophy

AssetIQ follows a simple principle:

> **Let the LLM understand and orchestrate. Let deterministic application code manage financial data and calculations.**

The LLM does not replace existing business logic.

Instead, it provides a natural-language interface over existing capabilities.

Semantic Kernel provides the orchestration layer.

MCP extends selected capabilities beyond the application boundary, allowing them to be exposed as reusable AI tools.

---

# Disclaimer

AssetIQ is an educational and experimental project demonstrating:

- Generative AI
- Semantic Kernel
- LLM Function Calling
- Tool Orchestration
- Model Context Protocol
- MCP Client / Server Architecture
- Portfolio Management Concepts

The portfolio data used in the project is sample data.

AssetIQ should not be used for actual investment, trading, or financial decision-making.

---

# Author

Built as a hands-on exploration of:

**C# + .NET + Capital Markets + Generative AI + Semantic Kernel + Model Context Protocol**

The project focuses on applying modern AI concepts to a realistic financial-domain use case.

AssetIQ demonstrates how existing enterprise C# business logic can evolve into AI-accessible capabilities without replacing the deterministic systems and business rules that already power enterprise applications.
