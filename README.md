# Desafio BackEnd

![alt text](Images/swagger.png)

### Sobre o desafio
Todos os casos de uso e diferenciais foram inclusos no projeto

Foi adicionado docker e docker composer para o projeto principal e para uso de um banco postgresql em dev/test que caso seja usado deve ser considerado `host.docker.internal` como host.

Foi adicionado uma pasta `k8s` com os scripts que foram utilizados para deploy no meu ambiente local do kubernets com Kind (Sistema, banco de dados, deployment, services, configmap e ingress), vale pontuar que no caso atual usei o kind-registry para gestão de imagens, com essa pasta você deve conseguir fazer deploy em qualquer ambiente local e cloud adaptando a camada de ingress

![alt text](Images/k9s.png)


## Iniciando o projeto
Para iniciar o projeto é necessário apenas que preencha o appsettings.json, para funcionar por completo deve adicionar também o .json de permissões do gcloud para conseguir usar o pub/sub e o storage/bucket

`Para os testes de integração externo é importante considerar /api/v1/ como parte da url base`

#### Aplicar Migrações
- cd Data
- dotnet ef database update --startup-project ..\BackEnd\ --context Context

## Oque foi utilizado?
- .NET 8.0
- EntityFrameworkCore 8
- Dapper
- PostgreSQL
- Google.Cloud.PubSub
- Google.Cloud.Storage
- xUnit

## Testes de Integração/Unitários
![alt text](Images/coverage.png)
Todos os end-points estão cobertos por testes de integração/middleware e foram feitos testes unitários para funções com cálculos mais críticos

## Diagrama do banco de dados
![alt text](Images/diagram-db.png)

## Considerações sobre Autenticação e Permissões
1. Como não ficou claro se um usuário pode ou não ter 2 permissões, considerei que não para demostrar melhor o controle de permissões.

2. Foi feito um end-point que só roda em debug para obter os JWT e testar as permissões, mas também deixei 2 JWT que podem ser usados se as chaves não forem alteradas.

3. A verificação de permissão está sendo feita pelo JWT então se for comum a troca de permissões é importante implementar um fluxo de cancelamento de token, a outra opção é verificar no banco a cada requisição se o usuário tem permissão, oque seria menos performático.
 
## Tokens
#### Administrador
`Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0YWYzNDkzNS0xODMwLTRjM2ItOThhZS04N2U0MWVlYTI1OTgiLCJ0eXBlIjoiMSIsIm5iZiI6MTcyOTM3OTEyMiwiZXhwIjoxNzYwOTE1MTIyLCJpYXQiOjE3MjkzNzkxMjJ9.aG6QyShYL11YPVH_gDVS-bmK3YqnJDkooOqtV6AiH40`

#### Entregador
`Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxNjUzNWUyNi1lNmNhLTRkZWEtOGUwZS0yYTQ1ZGQ1NDg3ZjYiLCJ0eXBlIjoiMiIsIm5iZiI6MTcyOTM3OTI4NiwiZXhwIjoxNzYwOTE1Mjg2LCJpYXQiOjE3MjkzNzkyODZ9.8vsLb1WbGsZLUg6METWvamaPNK19QRFNRSN79BX0kog`

## Informações Extras
`Para os testes de integração externo é importante considerar /api/v1/ como parte da url base`

1. Considerei os identificadores como strings cadastráveis já que não existe retorno de Id nem listagens em alguns casos para conseguir continuar usando o cadastro em outros end-points
2. Foi adicionado um campo de Identificador que não existe no swagger no post /locacao para que seja possível usar esse id no get /locacao/{id}
3. Não foi adicionado uma verificação se o Entregador que está solicitando é o mesmo que está autenticado  para não atrapalhar nos testes
4. Não fica claro na documentação como deve ser retornado o valor total da locação, então devolvi o total de dias, valor diário, valor de multa e total