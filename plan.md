# Discord TTRPG Bot

## Overview

Games supported:

- D&D 5e
- Shadowrun 6e

Features:

- Read & respond to chat
- Handle specific slash commands
- Have special ! commands for having the AI interact without necessarily using the slash commands

## Goals

1. Be able to get the bot to connect to a Discord server and respond to commands.
1. Get Discord to communicate with an LLM

## File Structure

- `/docs`: Documentation for the bot
- `/src`: Source code for the bot
  - `/src/TtrpgAiBot.Api`: The API for the bot
  - `/src/TtrpgAiBot.Core`: The core logic for the bot
  - `/src/TtrpgAiBot.Discord`: The Discord client for the bot
- `/tests`: Unit tests for the bot

- .Discord - The implementation to connection to Discord specifically

## Flow
