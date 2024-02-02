FROM mcr.microsoft.com/vscode/devcontainers/dotnet:8.0-bookworm-slim

ENV USER="vscode" \
    WORKDIR="/app" \
    NVM_DIR="/usr/local/share/nvm" \
    NVM_SYMLINK_CURRENT=true \
    PATH=${NVM_DIR}/current/bin:${PATH}

COPY .devcontainer/library-scripts/node-debian.sh /tmp/library-scripts/

RUN apt-get update && bash /tmp/library-scripts/node-debian.sh "${NVM_DIR}"

COPY --chown=${USER}:${USER} . ${WORKDIR}

USER ${USER}

WORKDIR ${WORKDIR}

RUN yarn &&\
    dotnet tool restore && \
    dotnet paket restore