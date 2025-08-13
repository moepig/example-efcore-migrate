FROM mcr.microsoft.com/devcontainers/base:noble

RUN apt-get update \
  && apt-get install -y --no-install-recommends \
  git \
  vim \
  dotnet-sdk-8.0

RUN update-alternatives --set editor /usr/bin/vim.basic

USER vscode
