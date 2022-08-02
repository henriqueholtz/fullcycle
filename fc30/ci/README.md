# Fullcycle3.0-ci

Este repositório é referente aos desafios propostos no módulo **CI - Continuous Integration** do curso **FullCycle 3.0**

## Desafio

- Crie uma pequena aplicação simples em node.js (qualquer aplicação mesmo)
- Crie testes de unidade para essa aplicação
- Crie uma pipeline de CI utilizando o Github actions que:
  - Instale a aplicação
  - Execute os testes
  - Execute o SonarCloud
- Suba essa aplicação para um repositório público no Gthub e ative o status check para que seja possível dar o merge apenas se a aplicação passar na pipeline de CI e também se a aplicação passar no quality gate do SonarCloud.
- Crie uma Pull Request (PR) realizando todo o processo.
- Informe o link da PR abaixo para que possamos corrigir.

### [PULL REQUEST](https://github.com/henriqueholtz/fullcycle3.0-ci/pull/1)

## Characteristics:

- :heavy_check_mark: **Typescript**
- :heavy_check_mark: **Testes com Jest**
- :heavy_check_mark: **CI com Github Actions**
  - :heavy_check_mark: **Instalação e execução dos testes com jest** (`--coverage` para gerar o resultado usado pelo SonarCloud)
  - :heavy_check_mark: **SonarCloud Scan**
- :heavy_check_mark: **Status Check na branch `master`**
- :heavy_check_mark: **Commit's assinados com chave GPG**
