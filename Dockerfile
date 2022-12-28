FROM mcr.microsoft.com/vscode/devcontainers/dotnet:6.0-bullseye-slim

ENV USER="vscode" \
    WORKDIR="/app"

COPY --chown=${USER}:${USER} . ${WORKDIR}

USER ${USER}

WORKDIR ${WORKDIR}
