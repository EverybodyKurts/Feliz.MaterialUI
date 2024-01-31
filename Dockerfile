FROM mcr.microsoft.com/vscode/devcontainers/dotnet:8.0-bookworm-slim

ENV USER="vscode" \
    WORKDIR="/app"

COPY --chown=${USER}:${USER} . ${WORKDIR}

USER ${USER}

WORKDIR ${WORKDIR}
