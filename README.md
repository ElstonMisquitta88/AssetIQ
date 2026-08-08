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
